#include "Windows.h"
#include "File.h"
#include <algorithm>
#include <cstdint>
#include <sys/stat.h>

#include <fstream>
#include <iostream>
#include <algorithm> 
#include <vector>
#include <string>

#include <sys/stat.h>

namespace Spek
{
	std::vector<std::string> FileFinder::m_Files;

	File::File(const std::string a_File) : FileFinder(a_File, "")
	{
		m_File.open(m_Filename.c_str(), std::ios::in | std::ios::binary | std::ios::ate);
		m_Loaded = m_File.is_open();
		m_File.seekg(0, m_File.beg);
	}

	File::~File()
	{
		m_File.close();
	}

	size_t File::GetSize() const
	{
		struct stat st;
		stat(m_Filename.c_str(), &st);
		return st.st_size;
	}

	std::string ExePath() 
	{
		char t_Buffer[MAX_PATH];
		GetModuleFileName(NULL, t_Buffer, MAX_PATH);
		std::string::size_type pos = std::string(t_Buffer).find_last_of("\\/");
		return std::string(t_Buffer).substr(0, pos);
	}

	void GetAllFoldersInDirectory(std::vector<std::string> &a_Out, const std::string &a_Dir)
	{
		HANDLE t_Dir;
		WIN32_FIND_DATA t_FileData;

		if ((t_Dir = FindFirstFile((a_Dir + "/*").c_str(), &t_FileData)) == INVALID_HANDLE_VALUE)
			return; /* No files found */

		do
		{
			const std::string t_FileName = t_FileData.cFileName;
			const std::string t_FullName = a_Dir + "/" + t_FileName;

			if (t_FileName[0] == '.')
				continue;

			if ((t_FileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0)
				a_Out.push_back(t_FileName);

		} while (FindNextFile(t_Dir, &t_FileData));

		FindClose(t_Dir);
	}

	void GetFilesInDirectory(std::vector<std::string> &a_Out, const std::string &a_Dir)
	{
		HANDLE t_Dir;
		WIN32_FIND_DATA t_FileData;

		if ((t_Dir = FindFirstFile((a_Dir + "/*").c_str(), &t_FileData)) == INVALID_HANDLE_VALUE)
			return; /* No files found */

		do 
		{
			const std::string t_FileName = t_FileData.cFileName;
			const std::string t_FullName = a_Dir + "/" + t_FileName;

			if (t_FileName[0] == '.')
				continue;

			if ((t_FileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0)
			{
				std::vector<std::string> t_Merge;
				GetFilesInDirectory(t_Merge, t_FullName);
				a_Out.insert(a_Out.end(), t_Merge.begin(), t_Merge.end());
			}
			else a_Out.push_back(t_FullName);
		} 
		while (FindNextFile(t_Dir, &t_FileData));

		FindClose(t_Dir);
	} 

	FileFinder::FileFinder(const std::string a_File, const std::string a_Folder)
	{
		m_Filename = a_File.c_str();
		m_Folder = a_Folder;
		std::atexit([]()
		{
			m_Files.clear();
		});
		LoopFind();
	}

	std::string FileFinder::BaseName()
	{
		return std::string
		(
			std::find_if(m_Filename.rbegin(), m_Filename.rend(),
			MatchPathSeparator()).base(),
			m_Filename.end()
		);
	}

	uint32_t g_FindUpper = 0;
	bool FileFinder::FindUpper(char i)
	{
		if(IsPathDivider(i)) 
			return g_FindUpper++==1;
		return false;
	}

	std::string FileFinder::GoUp(std::string a_Path)
	{
		g_FindUpper = 0; 
		return std::string
			(
			a_Path.begin(), std::find_if(a_Path.rbegin(), a_Path.rend(), FindUpper).base()
			);
	}

	std::string FileFinder::FilePath()
	{
		return std::string(m_Filename.begin(), std::find_if(m_Filename.rbegin(), m_Filename.rend(), MatchPathSeparator()).base());
	}

	std::string FileFinder::Content()
	{
		std::ifstream t_File;
		t_File.open(m_Filename);

		if (!t_File) { std::cerr << "Can not open file: \"" << m_Filename << "\"" << std::endl; return ""; }

		std::string t_Content(std::istreambuf_iterator<char>(t_File), (std::istreambuf_iterator<char>()));
		t_File.close();

		return t_Content;
	}

	inline bool FileFinder::Exists() 
	{
		struct stat buffer;
		return (stat(m_Filename.c_str(), &buffer) == 0);
	}
	
	
	bool FileFinder::HasEnding(std::string a_Path)
	{
		if (a_Path.size() == 0) return false;
		return IsPathDivider(a_Path.back());
	}

	bool FileFinder::LoopFind()
	{
		//if(m_Files.size()==0) 
		GetFilesInDirectory(m_Files, m_Folder);//ExePath());

		int32_t iterations = 0;
		std::string t_OldName = m_Filename;
		//if(!Exists())
		{
			std::string t_Path = FilePath();
			std::string t_FileName = BaseName();

			for (std::string t_File : m_Files) // Oh lord
			{
				auto t_Dest = t_File.find(t_FileName);
				if (t_Dest != std::string::npos)
				{

					auto t_Dest = t_File.find("Particles");
					if (t_Dest == std::string::npos)
					//m_Filename = t_File;
					//if (Exists())
						m_ResultFiles.push_back(t_File);
				}
			}
		}

		return m_ResultFiles.size()>0;
	}
}