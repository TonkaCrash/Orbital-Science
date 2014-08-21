﻿#region license
/* DMagic Orbital Science - DMCollectScience
 * Contract Parameter for science collection
 *
 * Copyright (c) 2014, David Grandy <david.grandy@gmail.com>
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 * 
 * 2. Redistributions in binary form must reproduce the above copyright notice, 
 * this list of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used 
 * to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
 * OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *  
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Parameters;

namespace DMagic
{
	public class DMCollectScience : CollectScience
	{
		private CelestialBody body;
		private ExperimentSituations scienceLocation;
		private DMScienceContainer scienceContainer;
		private string subject, name, biomeName, aSize, partName;
		private bool collected;
		private int size;
		private int type; //type 0: standard experiment; type 1: standard survey; type 2: asteroid survey; type 3: anomaly

		public DMCollectScience()
		{
		}

		internal DMCollectScience(CelestialBody target, ExperimentSituations location, string BiomeName, string Name, int Type)
		{
			body = target;
			scienceLocation = location;
			name = Name;
			biomeName = BiomeName;
			type = Type;
			collected = true;
			DMUtils.availableScience["All"].TryGetValue(name, out scienceContainer);
			partName = scienceContainer.sciPart;
			subject = string.Format("{0}@{1}{2}{3}", scienceContainer.exp.id, body.name, scienceLocation, biomeName.Replace(" ", ""));
		}

		internal DMCollectScience(int Size, ExperimentSituations Location, string Name, int Type)
		{
			body = null;
			scienceLocation = Location;
			name = Name;
			type = Type;
			size = Size;
			aSize = DMUtils.sizeHash(size);
			biomeName = "";
			collected = false;
			DMUtils.availableScience["All"].TryGetValue(name, out scienceContainer);
			partName = scienceContainer.sciPart;
			subject = string.Format("{0}@Asteroid{1}{2}", scienceContainer.exp.id, scienceLocation, biomeName);
		}

		/// <summary>
		/// Used externally to return the name of the requested part
		/// </summary>
		/// <param name="cP">Instance of the requested Contract Parameter</param>
		/// <returns>Available Part name string</returns>
		public static string PartName(ContractParameter cP)
		{
			DMCollectScience Instance = (DMCollectScience)cP;
			return Instance.partName;
		}

		//Properties to be accessed by parent contract
		internal CelestialBody Body
		{
			get { return body; }
			private set { }
		}

		internal ExperimentSituations Situation
		{
			get { return scienceLocation; }
			private set { }
		}

		internal string Biome
		{
			get { return biomeName; }
			private set { }
		}

		internal string Subject
		{
			get { return subject; }
			private set { }
		}

		internal DMScienceContainer Container
		{
			get { return scienceContainer; }
			private set { }
		}

		internal string Name
		{
			get { return name; }
			private set { }
		}

		protected override string GetHashString()
		{
			return subject;
		}

		protected override string GetTitle()
		{
			if (type == 2)
			{
				if (scienceLocation == ExperimentSituations.InSpaceLow)
					return string.Format("Collect {0} data from in space near a {1} asteroid", scienceContainer.exp.experimentTitle, aSize);
				else if (scienceLocation == ExperimentSituations.SrfLanded)
					return string.Format("Collect {0} data while grappled to a {1} asteroid", scienceContainer.exp.experimentTitle, aSize);
				else
					return "Stupid Code Is Stupid";
			}
			else if (type == 3)
			{
				if (scienceLocation == ExperimentSituations.SrfLanded)
					
					return string.Format("Gather {0} data from the surface near the anomalous signal emanating from {1}", scienceContainer.exp.experimentTitle, body.theName);
				else if (scienceLocation == ExperimentSituations.FlyingLow)
					return string.Format("Gather {0} data while flying above the anomalous signal emanating from {1}", scienceContainer.exp.experimentTitle, body.theName);
				else
					return "Stupid Code Is Stupid";
			}
			else
			{
				if (!string.IsNullOrEmpty(biomeName))
				{
					if (scienceLocation == ExperimentSituations.InSpaceHigh)
						return string.Format("Collect {0} data from high orbit around {1}'s {2}", scienceContainer.exp.experimentTitle, body.theName, biomeName);
					else if (scienceLocation == ExperimentSituations.InSpaceLow)
						return string.Format("Collect {0} data from low orbit around {1}'s {2}", scienceContainer.exp.experimentTitle, body.theName, biomeName);
					else if (scienceLocation == ExperimentSituations.SrfLanded)
						return string.Format("Collect {0} data from the surface at {1}'s {2}", scienceContainer.exp.experimentTitle, body.theName, biomeName);
					else if (scienceLocation == ExperimentSituations.SrfSplashed)
						return string.Format("Collect {0} data from the oceans at {1}'s {2}", scienceContainer.exp.experimentTitle, body.theName, biomeName);
					else if (scienceLocation == ExperimentSituations.FlyingHigh)
						return string.Format("Collect {0} data during high altitude flight over {1}'s {2}", scienceContainer.exp.experimentTitle, body.theName, biomeName);
					else if (scienceLocation == ExperimentSituations.FlyingLow)
						return string.Format("Collect {0} data during low altitude flight over {1}'s {2}", scienceContainer.exp.experimentTitle, body.theName, biomeName);
				}
				else
				{
					if (scienceLocation == ExperimentSituations.InSpaceHigh)
						return string.Format("Collect {0} data from high orbit around {1}", scienceContainer.exp.experimentTitle, body.theName);
					else if (scienceLocation == ExperimentSituations.InSpaceLow)
						return string.Format("Collect {0} data from low orbit around {1}", scienceContainer.exp.experimentTitle, body.theName);
					else if (scienceLocation == ExperimentSituations.SrfLanded)
						return string.Format("Collect {0} data from the surface of {1}", scienceContainer.exp.experimentTitle, body.theName);
					else if (scienceLocation == ExperimentSituations.SrfSplashed)
						return string.Format("Collect {0} data from the oceans of {1}", scienceContainer.exp.experimentTitle, body.theName);
					else if (scienceLocation == ExperimentSituations.FlyingHigh)
						return string.Format("Collect {0} data during high altitude flight at {1}", scienceContainer.exp.experimentTitle, body.theName);
					else if (scienceLocation == ExperimentSituations.FlyingLow)
						return string.Format("Collect {0} data during low altitude flight at {1}", scienceContainer.exp.experimentTitle, body.theName);
				}
				return "Stupid Code Is Stupid";
			}
		}

		protected override void OnRegister()
		{
			GameEvents.OnScienceRecieved.Add(scienceRecieve);
		}

		protected override void OnUnregister()
		{
			GameEvents.OnScienceRecieved.Remove(scienceRecieve);
		}

		protected override void OnSave(ConfigNode node)
		{
			//DMUtils.DebugLog("Saving Contract Parameter");
			if (type == 2)
				node.AddValue("Science_Subject", string.Format("{0}|{1}|{2}|{3}|{4}|{5}", type, name, size, (int)scienceLocation, "", collected));
			else
				node.AddValue("Science_Subject", string.Format("{0}|{1}|{2}|{3}|{4}|{5}", type, name, body.flightGlobalsIndex, (int)scienceLocation, biomeName, collected));
		}

		protected override void OnLoad(ConfigNode node)
		{
			//DMUtils.DebugLog("Loading Contract Parameter");
			int targetBodyID, targetLocation;
			string[] scienceString = node.GetValue("Science_Subject").Split('|');
			if (!int.TryParse(scienceString[0], out type))
			{
				DMUtils.Logging("Failed To Load Contract Parameter; Parameter Removed");
				this.Root.RemoveParameter(this);
			}
			name = scienceString[1];
			DMUtils.availableScience["All"].TryGetValue(name, out scienceContainer);
			partName = scienceContainer.sciPart;
			if (int.TryParse(scienceString[3], out targetLocation))
				scienceLocation = (ExperimentSituations)targetLocation;
			else
			{
				DMUtils.Logging("Failed To Load Variables; Parameter Removed");
				this.Root.RemoveParameter(this);
			}
			biomeName = scienceString[4];
			if (!bool.TryParse(scienceString[5], out collected))
			{
				DMUtils.Logging("Failed To Load Variables; Parameter Removed");
				this.Root.RemoveParameter(this);
			}
			if (type == 2)
			{
				if (int.TryParse(scienceString[2], out size))
					aSize = DMUtils.sizeHash(size);
				else
				{
					DMUtils.Logging("Failed To Load Contract Parameter; Parameter Removed");
					this.Root.RemoveParameter(this);
				}
				subject = string.Format("{0}@Asteroid{1}{2}", scienceContainer.exp.id, scienceLocation, biomeName);
			}
			else
			{
				if (int.TryParse(scienceString[2], out targetBodyID))
					body = FlightGlobals.Bodies[targetBodyID];
				else
				{
					DMUtils.Logging("Failed To Load Variables; Parameter Removed");
					this.Root.RemoveParameter(this);
				}
				subject = string.Format("{0}@{1}{2}{3}", scienceContainer.exp.id, body.name, scienceLocation, biomeName.Replace(" ", ""));
			}

		}

		protected override void OnUpdate()
		{
			if (type == 2)
			{
				if (this.Root.ContractState == Contract.State.Active && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
				{
					if (!collected)
					{
						if (setAstVessel(DMUtils.astSize, DMUtils.newAstExp))
						{
							ScreenMessages.PostScreenMessage("Asteroid Science Results Collected", 6f, ScreenMessageStyle.UPPER_CENTER);
							collected = true;
							DMUtils.astSize = "";
							DMUtils.newAstExp = "";
						}
					}
				}
			}
		}

		private bool setAstVessel(string s, string e)
		{
			if (string.IsNullOrEmpty(s))
				return false;
			else
			{
				if (e == scienceContainer.exp.id)
				{
					if (s == aSize)
						return true;
					else
						return false;
				}
				else
					return false;
			}
		}

		private void scienceRecieve(float sci, ScienceSubject sub)
		{
			DMUtils.DebugLog("New Science Results Collected With ID: {0}", sub.id);
			DMUtils.DebugLog("Comparing To Target Science With ID: {0}", subject);
			if (type == 0 || type == 3)
			{
				DMUtils.DebugLog("Checking Science Results For Type [{0}] Contract", type);
				if (!string.IsNullOrEmpty(biomeName))
				{
					if (sub.id == subject)
					{
						DMUtils.DebugLog("Contract Complete");
						base.SetComplete();
					}
				}
				else
				{
					string clippedSub = sub.id.Replace("@", "");
					string clippedTargetSub = subject.Replace("@", "");
					DMUtils.DebugLog("Comparing New Strings [{0}] And [{1}]", clippedSub, clippedTargetSub);
					if (clippedSub.StartsWith(clippedTargetSub))
					{
						if (sci < (scienceContainer.exp.baseValue * scienceContainer.transmit * sub.subjectValue * 0.4f))
							ScreenMessages.PostScreenMessage("This area has already been studied, try investigating another region to complete the contract", 8f, ScreenMessageStyle.UPPER_CENTER);
						else
						{
							DMUtils.DebugLog("Contract Complete");
							base.SetComplete();
						}
					}
				}
			}
			else if (type == 1)
			{
				DMUtils.DebugLog("Checking Science Results For Type [{0}] Contract", type);
				if (!string.IsNullOrEmpty(biomeName))
				{
					if (sub.id == subject)
					{
						DMUtils.DebugLog("Contract Complete");
						base.SetComplete();
					}
				}
				else
				{
					string clippedSub = sub.id.Replace("@", "");
					string clippedTargetSub = subject.Replace("@", "");
					DMUtils.DebugLog("Comparing New Strings [{0}] And [{1}]", clippedSub, clippedTargetSub);
					if (clippedSub.StartsWith(clippedTargetSub))
					{
						DMUtils.DebugLog("Contract Complete");
						base.SetComplete();
					}
				}
			}
			else if (type == 2)
			{
				if (collected)
				{
					DMUtils.DebugLog("Checking Science Results For Type [{0}] Contract", type);
					if (!string.IsNullOrEmpty(biomeName))
					{
						if (sub.id == subject)
						{
							DMUtils.DebugLog("Contract Complete");
							base.SetComplete();
						}
					}
					else
					{
						string clippedSub = sub.id.Replace("@", "");
						string clippedTargetSub = subject.Replace("@", "");
						DMUtils.DebugLog("Comparing New Strings [{0}] And [{1}]", clippedSub, clippedTargetSub);
						if (clippedSub.StartsWith(clippedTargetSub))
						{
							DMUtils.DebugLog("Contract Complete");
							base.SetComplete();
						}
					}
				}
			}
		}

	}
}
