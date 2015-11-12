#include "stdafx.h"
#include "lol2daeDlg.h"
#include "ColladaWriter.h"
#include "SknImporter.h"
#include "SklImporter.h"
#include "AnmImporter.h"
#include "file.h"

#include <sys/stat.h>
bool CreateDAE(const std::string& a_Out, const std::string& a_Name, const std::string& a_Skin, const std::string& a_Skeleton, Spek::FileFinder& a_AnimationFiles);

int main()
{
	// Get all folders inside CHARACTERS
	std::string t_BaseFolder = Spek::ExePath() + "/../DATA/Characters";
	std::vector<std::string> t_FolderList;

	//Spek::GetAllFoldersInDirectory(t_FolderList, t_BaseFolder);

	t_FolderList.push_back("CaitlynTrap");

	// for each 
	for (int i = 0; i < t_FolderList.size(); i++)
	{
		std::cout << "[" + std::to_string(i) + '/' + std::to_string(t_FolderList.size()) + "] Working on " + t_FolderList[i] + "." << std::endl;

		// Get the skl file
		Spek::FileFinder t_SkeletonFiles = Spek::FileFinder(".skl", t_BaseFolder + "/" + t_FolderList[i] + "/Skins/Base");

		if (t_SkeletonFiles.GetAllFiles().size() >= 1)
		{
			// Get the skn file
			Spek::FileFinder t_SkinFiles = Spek::FileFinder(".skn", t_BaseFolder + "/" + t_FolderList[i] + "/Skins/Base");

			// Get all anm files
			Spek::FileFinder t_AnimationFiles = Spek::FileFinder(".anm", t_BaseFolder + "/" + t_FolderList[i] + "/Skins/Base");
		
			std::string t_Out = Spek::ExePath() + "/output/";// +t_FolderList[i];

			// Create
			if (CreateDAE(t_Out, t_FolderList[i],
				t_SkinFiles.GetAllFiles()[0],
				t_SkeletonFiles.GetAllFiles()[0],
				t_AnimationFiles))
			{
				std::cout << "Created DAE for " << t_FolderList[i] << "." << std::endl;
			}
			else
			{
				std::cout << "Failed to create DAE for " << t_FolderList[i] << "." << std::endl;
			}
		}
		else
		{
			std::cout << "Cannot find any regular structure for the '" + t_FolderList[i] + "' folder." << std::endl;
		}
	}
}

bool CreateDAE(const std::string& a_Out, const std::string& a_Name, const std::string& a_Skin, const std::string& a_Skeleton, Spek::FileFinder& a_AnimationFiles)
{
	ColladaWriter::Mode mode = ColladaWriter::Mode::Mesh;

	auto t_Animations = a_AnimationFiles.GetAllFiles();

	try
	{
		for (int i = 0; i < t_Animations.size(); i++)
		{
			std::cout << "Importing skin.." << std::endl;
			SknImporter inputSkn;
			inputSkn.readFile(a_Skin.c_str());

			std::cout << "Importing skeleton.." << std::endl;
			SklImporter inputSkl(inputSkn.fileVersion);
			std::vector<AnmImporter> inputAnm;

			for (int i = 0; i < t_Animations.size(); i++)
				inputAnm.push_back(AnmImporter(inputSkl.boneHashes));

			//if (mode >= ColladaWriter::Mode::Skeleton)
			{
				inputSkl.readFile(a_Skeleton.c_str());

				if (t_Animations.size() > 0) mode = ColladaWriter::Mode::Animation;

				if (mode == ColladaWriter::Mode::Animation)
				{
					std::cout << "Reading " << t_Animations.size() << " animations.." << std::endl;
					//std::cout << "Reading animation " << i << '/' << a_AnimationFiles.size() << "." << std::endl;
					inputAnm[i].readFile(t_Animations[i].c_str());
				}
			}

			std::string t_CleanName = 
				
				std::string
				(
					std::find_if(t_Animations[i].rbegin(), t_Animations[i].rend(),
								Spek::FileFinder::MatchPathSeparator()).base(),
					t_Animations[i].end()-4
				) + ".dae";

			std::cout << "Writing to Collada file." << std::endl;
			ColladaWriter outputCollada(a_Name, inputSkn.indices, inputSkn.vertices, inputSkl.bones, inputSkl.boneIndices, inputAnm[i]);
			outputCollada.writeFile((a_Out + t_CleanName).c_str(), mode);
			std::cout << "Written." << std::endl;
		}

		return true;
	}

	catch (lol2daeError& e)
	{
		std::cout << e.what() << std::endl;
		return false;
	}
}