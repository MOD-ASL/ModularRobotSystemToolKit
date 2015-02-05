/*
 * Copyright 2015 by Gangyuan Jing
 *
 * This file is part of ModularSystemToolKit.
 *
 * ModularSystemToolKit is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * ModularSystemToolKit is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with ModularSystemToolKit.  If not, see
 * <http://www.gnu.org/licenses/>.*
*/

/*
 * Author: Gangyuan Jing
 * Description: A model plugin for SMORES_MODULE model in gazebo
*/

#ifndef MRSTK_GAZEBO_PLUGINS_MODEL_SM_CONTROLLER_HH_
#define MRSTK_GAZEBO_PLUGINS_MODEL_SM_CONTROLLER_HH_

#include <boost/bind.hpp>
#include <boost/log/trivial.hpp>
#include <math.h>
#include <gazebo/gazebo.hh>
#include <gazebo/physics/physics.hh>
#include <gazebo/common/common.hh>


typedef std::vector<gazebo::physics::JointPtr> Joint_V;


namespace gazebo
{
  class SmoresModuleController : public ModelPlugin 
  {
    public:
      void Load(physics::ModelPtr _parent, sdf::ElementPtr /*_sdf*/);

    private:
      void LoadJointController(void);
      physics::JointPtr GetJointByName(const std::string joint_name);
    
    private:
      physics::ModelPtr model;
      physics::JointControllerPtr joint_controller;

  }; // class SmoresModuleController
} // namespace gazebo

#endif // MRSTK_GAZEBO_PLUGINS_MODEL_SM_CONTROLLER_HH_
