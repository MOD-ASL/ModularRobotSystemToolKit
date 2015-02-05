#!/usr/bin/env sh

if [ $# -eq 1 ]; then
    if [ "$1" = "-m" ]; then
        if [ ! -d $HOME/.gazebo/models ]; then
            echo "Cannot find models folder, creating $HOME/.gazebo/models"
            mkdir $HOME/.gazebo/models
        fi
        echo "Copying SMORES_MODULE to $HOME/.gazebo/models"
        cp -r ./gazebo/models/SMORES_MODULE/ $HOME/.gazebo/models/
    fi
    if [ "$1" = "-p" ]; then
        if [ ! -d $HOME/.gazebo/models/SMORES_MODULE/plugins ]; then
            echo "Cannot find plugins folder, creating
            $HOME/.gazebo/models/SMORES_MODULE/plugins"
            mkdir $HOME/.gazebo/models/SMORES_MODULE/plugins
        fi
        echo "Compiling and copying model plugin to
        $HOME/.gazebo/models/SMORES_MODULE/plugins"
        cd $HOME/ModularRobotSystemToolKit/gazebo/plugins/model/SMORES_MODULE_controller/
        if [ ! -d build ]; then
            mkdir build 
        fi
        cd build
        cmake ../
        make
        cp libSMORES_MODULE_controller.so $HOME/.gazebo/models/SMORES_MODULE/plugins
    fi
fi
