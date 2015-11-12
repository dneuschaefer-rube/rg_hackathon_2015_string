#pragma once

struct FileLocation {
	char Path[_MAX_PATH];
	char Drive[_MAX_DRIVE];
	char Directory[_MAX_DIR];
	char Name[_MAX_FNAME];
	char Extension[_MAX_EXT];
};