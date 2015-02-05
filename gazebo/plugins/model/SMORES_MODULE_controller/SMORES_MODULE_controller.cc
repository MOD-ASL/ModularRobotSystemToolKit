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

#include "SMORES_MODULE_controller.hh"

namespace gazebo
{
  void SmoresModuleController::Load(physics::ModelPtr _parent, sdf::ElementPtr
      /*_sdf*/)
  {
    // Store the pointer to the model
    this->model = _parent;

    // Load a joint controller
    LoadJointController();

    physics::JointPtr test = GetJointByName("hinge");

  } // SmoresModuleController::Load

  void SmoresModuleController::LoadJointController(void)
  {
    this->joint_controller = physics::JointControllerPtr(new
        physics::JointController(this->model));

    for (physics::Joint_V::const_iterator it = this->model->GetJoints().begin();
        it != this->model->GetJoints().end(); ++it)
    {
      BOOST_LOG_TRIVIAL(debug) << "Adding joint " + (*it)->GetName();
      this->joint_controller->AddJoint(*it);
    }
  } // SmoresModuleController::LoadJointController

  physics::JointPtr SmoresModuleController::GetJointByName(const std::string
      joint_name)
  {
    physics::JointPtr found_joint;
    std::map< std::string, physics::JointPtr > joints =
      this->joint_controller->GetJoints();
    std::map< std::string, physics::JointPtr >::iterator it;

    it = joints.find(joint_name);
    if ( it == joints.end() )
    {
      BOOST_LOG_TRIVIAL(error) << "Cannot find joint with name " + joint_name;
      PrintJointNames();
    }
    else
    {
      found_joint = it->second;
    }
    return found_joint;
  } // SmoresModuleController::GetJointByName

  void SmoresModuleController::PrintJointNames(void)
  {
    BOOST_LOG_TRIVIAL(info) << "The available joints are ...";
    std::map< std::string, physics::JointPtr > joints =
      this->joint_controller->GetJoints();
    std::map< std::string, physics::JointPtr >::iterator it;

    for (it=joints.begin(); it!=joints.end(); ++it)
    {
      BOOST_LOG_TRIVIAL(info) << it->first;
    }
  } // SmoresModuleController::PrintJointNames

  GZ_REGISTER_MODEL_PLUGIN(SmoresModuleController)
} // namespace gazebo
