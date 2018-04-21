#!/bin/sh


if [ ! -f Dependencies.zip ]; then
	echo "Fetching Dependencies"
	wget "https://github.com/cookgreen/AMGE/releases/download/alpha-0.1.1/Dependencies.zip"
	unzip Dependencies.zip -d Dependencies
	rm -rf Dependencies.zip