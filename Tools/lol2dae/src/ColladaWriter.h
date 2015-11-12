#pragma once
#include "stdafx.h"
#include "AnmImporter.h"

class ColladaWriter
{
public:
	enum Mode
	{
		Mesh = 1,
		Skeleton,
		Animation
	};

private:
	const char* filePath;
	const vector<short>& indices;
	const vector<SknVertex>& vertices;
	vector<SklBone>& bones;
	const vector<int>& boneIndices;
	const AnmImporter& animation;
	list<int> nodes;
	ostringstream nodeStream;
	std::string name;

	bool emplaceNode(int boneId);
	void createNodeStringStream();
	void writeHeader();
	void writeMaterial();
	void writeMesh();
	void writeSkin();
	void writeScene(Mode& mode);
	void writeAnimation();

public:
	void writeFile(const char* path, Mode& mode);
	ColladaWriter(const std::string& name, vector<short>& indices, vector<SknVertex>& vertices, vector<SklBone>& bones, vector<int>& boneIndices, AnmImporter& animation)
		: name(name), indices(indices), vertices(vertices), bones(bones), boneIndices(boneIndices), animation(animation){};
	~ColladaWriter();
};

