#!/usr/bin/env bash

source ./CONFIG.inc

VERSIONFILE=$PACKAGE.version

scp -i $SSH_ID "./GameData/$TARGETDIR/$VERSIONFILE" $SITE:/$TARGET_CONTENT_PATH
scp -i $SSH_ID "./GameData/$TARGETDIR/$PACKAGE.README.md" $SITE:/${TARGET_CMS_PATH}${PACKAGE}/index.md
#scp -i $SSH_ID "./PR_material/banner.jpg" $SITE:/${TARGET_CONTENT_PATH}PR_material/banner.jpg
