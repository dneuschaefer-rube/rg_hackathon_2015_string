#pragma once 

#include <string>
#include <vector>
#include <iostream>
#include <fstream>

namespace Spek
{
	class FileFinder
	{
	public:
		FileFinder(const std::string a_File, const std::string a_Folder);

		operator std::string() { return m_Filename; }
		std::string BaseName();
		std::string FilePath();
		std::string Content();

		std::vector<std::string>& GetAllFiles() { return m_ResultFiles; }
	//protected:
#ifdef _WIN32
		static inline bool IsPathDivider(char ch) { return ch == '\\' || ch == '/'; }
		struct MatchPathSeparator
		{
			inline bool operator()(char ch) const
			{
				return IsPathDivider(ch);
			}
		};
#else
#if defined(unix) || defined(__unix__) || defined(__unix)
		static inline bool IsPathDivider(char ch) { return ch == '/'; }
		struct MatchPathSeparator
		{
			inline bool operator()(char ch) const
			{
				return IsPathDivider(ch);
			}
		};
#else 
#error Can't seem to find out what OS this is..
#endif
#endif
		std::string FileFinder::GoUp(std::string a_Path);
		std::string m_Filename;

		std::vector<std::string> m_ResultFiles;

		std::string m_Folder;
		static std::vector<std::string> m_Files;
		bool LoopFind();
		inline bool Exists(); 
		static bool FindUpper(char i); 
		static bool HasEnding(std::string a_Path);
	};

	void GetAllFoldersInDirectory(std::vector<std::string> &a_Out, const std::string &a_Dir);
	std::string ExePath();

	class File : public FileFinder
	{
	public:
		File(const std::string a_File);
		File(const Spek::File& a_File);
		~File();

		bool Loaded() const { return m_Loaded; }

		size_t GetSize() const;

		template<class T>
		bool Read(T* a_Buffer, size_t a_Size = NULL)
		{
			if (Loaded())
			{
				m_File.read((char*)a_Buffer, ((a_Size == NULL) ? (sizeof(T)) : (sizeof(T)*a_Size)));
				if (m_File) return true;
				else return false;
			}
			return false;
		}
	private:
		bool m_Loaded = false;
		std::ifstream m_File;
	};
}