#pragma once

class SklImporter
{
private:
	int fileVersion;
	const short& sknVersion;

	unsigned int StringToHash(string s);

public:
	int numBones;
	vector<SklBone> bones;
	int numBoneIndices;
	vector<int> boneIndices;
	map<unsigned int, char*> boneHashes;

	void readFile(const char* path);
	SklImporter(short& sknVersion) : sknVersion(sknVersion){};
	~SklImporter();
};

