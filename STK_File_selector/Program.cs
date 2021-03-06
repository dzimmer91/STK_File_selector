//program.cs :

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Text;
using System.Data;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using AGI.STKUtil;
using AGI.STKObjects;
using AGI.Ui.Application;
using System.Drawing;
using AGI.STKVgt;


namespace STK_File_selector
{



    partial class NPAS_To_STK
    {

        #region Class Members


        private AgUiApplication m_oSTK;
        private IAgStkObjectRoot m_oApplication;
        private int
            orbitmissioncount,
            stationcount,
            tlcount;
        private string
            NPAS_Mask_File,
            NPAS_Orbit_File,
            NPAS_Timeline_File,
            Tmp_directory,
            Efile_directory,
            startdate,
            enddate;
        private bool
            Tdrss_enabled,
            failure,
            groundtrack_displayed;


        string[] missionindex;


        public struct timeline_str
        {
            public string
                mis,
                sta,
                band,
                link,
                aosdate,
                aostime,
                losdate,
                lostime,
                duration;

        };
        public struct orbit_str
        {
            public bool
                efileused;
            public int
                misnum,
                cod_id,
                centerbody,
                endopt,
                duration,
                used;
            public string
                name,
                epoch,
                epoch_time,
                start_date,
                start_time,
                end_date,
                end_time,
                efilename;
            public double
                sma,
                ecc,
                inc,
                raan,
                aop,
                ma;

            public IAgChain MisChain;
            public IAgSatellite MisSat;
            public IAgSensor Missensor;
          //  public timeline_str[] mistldata;
        };
        public struct station_str
        {
            public bool
                used,
                tdrs;
            public string
                name;

            public double
                lat,
                lon,
                altitude;
           // public IAgFacility StaFacility;
        };

        station_str[] stationdata;
        orbit_str[] orbitdata;
        timeline_str[] tldata;



        unsafe struct CMWriteStr
        {//TODO should be moved to top
         //create struct to decode bin file from NPAS
         //byte is used instead of char because char size is 2 in UTF-16


            public int misnum;
            public fixed byte misname[20];
            public fixed byte epoch_ddmmyyyy[11];
            public fixed byte epoch_hhmmsss[13];
            public fixed byte start_ddmmyyyy[11];
            public fixed byte start_hhmmsss[13];

            public fixed byte end_ddmmyyyy[11];
            public fixed byte end_hhmmsss[13];

            public int efile_opt;
            public fixed byte efile_name[200];

            public int duration;

            public int endopt;
            public int epbod,
                epcor,
                epelm;

            public double epel_01,
                   epel_02,
                   epel_03,
                   epel_04,
                   epel_05,
                   epel_06;
            public int cod_id;

        };

        #endregion

        #region Constructor
        public NPAS_To_STK()
        {
            missionindex = new string[200];

            try
            {
                m_oSTK = Marshal.GetActiveObject("STK11.Application") as AGI.Ui.Application.AgUiApplication;
                Console.Write("Looking for an instance of STK... ");
            }
            catch
            {
                Console.Write("Creating a new STK 11 instance... ");
                Guid clsID = typeof(AgUiApplicationClass).GUID;
                Type oType = Type.GetTypeFromCLSID(clsID);
                m_oSTK = Activator.CreateInstance(oType) as AGI.Ui.Application.AgUiApplication;
                try
                {
                    m_oSTK.LoadPersonality("STK");
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    Console.WriteLine("Error");
                    Console.WriteLine(ex.Message);
                //  Console.WriteLine("Press any key to continue . . .");
                //  Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            try
            {
                m_oApplication = (IAgStkObjectRoot)m_oSTK.Personality2;
                Console.WriteLine("done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        #endregion

        private void find_STK()
        {
            try
            {
                m_oSTK = Marshal.GetActiveObject("STK11.Application") as AGI.Ui.Application.AgUiApplication;
                Console.Write("Looking for an instance of STK... ");
            }
            catch
            {
                Console.Write("Creating a new STK 11 instance... ");
                Guid clsID = typeof(AgUiApplicationClass).GUID;
                Type oType = Type.GetTypeFromCLSID(clsID);
                m_oSTK = Activator.CreateInstance(oType) as AGI.Ui.Application.AgUiApplication;
                try
                {
                    m_oSTK.LoadPersonality("STK");
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    Console.WriteLine("Error");
                    Console.WriteLine(ex.Message);
                    //Console.WriteLine("Press any key to continue . . .");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            try
            {
                m_oApplication = (IAgStkObjectRoot)m_oSTK.Personality2;
                Console.WriteLine("done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.Message);
                //Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public station_str[] Rtn_stationdata()
        {
            return stationdata;
        }
        public orbit_str[] Rtn_orbitdata()
        {
            return orbitdata;
        }

        public void set_temp_directory(string dirname)
        {
            Tmp_directory = dirname;
        }

        public void set_efile_directory(string dirname)
        {
            Efile_directory = dirname;
        }

        public void set_timeline_file(string filename)
        {
            NPAS_Timeline_File = filename;
        }

        public void set_mask_file(string filename)
        {
            NPAS_Mask_File = filename;
        }

        public void set_orbit_file(string filename)
        {
            NPAS_Orbit_File = filename;
        }

        public void set_groundtrack(bool passed)
        {
            groundtrack_displayed = passed;
        }

        //generate target sensor at 2 degree cone angle
        private IAgSensor generate_sensor(string missionname, string constellation_name)
        {
            IAgSensor sensorptr;
            //create a new sensor for the current sat IAgSnSimpleConicPattern
            sensorptr = (IAgSensor)m_oApplication.CurrentScenario.Children[missionname].Children.New(AgESTKObjectType.eSensor, missionname + "_sensor");
            sensorptr.SetPointingType(AgESnPointing.eSnPtTargeted);

            IAgSnPtTargeted test12 = (IAgSnPtTargeted)sensorptr.Pointing;
            test12.Targets.Add("/Constellation/"+ constellation_name);

            IAgSnSimpleConicPattern simpleConic = (IAgSnSimpleConicPattern)sensorptr.Pattern;
            //set a 2deg half cone angle for pointing at an object
            simpleConic.ConeAngle = 2;
            return sensorptr;
        }
        //generate target sensor at provided coneangle
        private IAgSensor generate_sensor(string missionname, string constellation_name, int coneangle)
        {
            IAgSensor sensorptr;
            //create a new sensor for the current sat IAgSnSimpleConicPattern
            sensorptr = (IAgSensor)m_oApplication.CurrentScenario.Children[missionname].Children.New(AgESTKObjectType.eSensor, missionname + "_sensor");
            sensorptr.SetPointingType(AgESnPointing.eSnPtTargeted);

            IAgSnPtTargeted test12 = (IAgSnPtTargeted)sensorptr.Pointing;
            test12.Targets.Add("/Constellation/" + constellation_name);

            IAgSnSimpleConicPattern simpleConic = (IAgSnSimpleConicPattern)sensorptr.Pattern;
            //set a 2deg half cone angle for pointing at an object
            simpleConic.ConeAngle = coneangle;
            return sensorptr;
        }
        //generate sensor with pointertype being provided
        private IAgSensor generate_sensor(string missionname, string constellation_name, int coneangle, AgESnPointing pttype)
        {
            IAgSensor sensorptr;
            //create a new sensor for the current sat IAgSnSimpleConicPattern
            sensorptr = (IAgSensor)m_oApplication.CurrentScenario.Children[missionname].Children.New(AgESTKObjectType.eSensor, missionname + "_sensor");
            sensorptr.SetPointingType(pttype);

            if (sensorptr.PointingType == AgESnPointing.eSnPtTargeted)
            {
                IAgSnPtTargeted localtargetPTtype = (IAgSnPtTargeted)sensorptr.Pointing;
                localtargetPTtype.Targets.Add("/Constellation/" + constellation_name);
            }
            /*
            else if (sensorptr.PointingType == AgESnPointing.eSnPtFixed)
            {
                IAgSnPtFixed localfixPtType = (IAgSnPtFixed)sensorptr.Pointing;
                IAgOrientation localorientation =(IAgOrientation)localfixPtType.Orientation;
                localorientation.


            }*/
            IAgSnSimpleConicPattern simpleConic = (IAgSnSimpleConicPattern)sensorptr.Pattern;
            //set a 2deg half cone angle for pointing at an object
            simpleConic.ConeAngle = coneangle;
            return sensorptr;
        }

        private IAgChain generate_chain(string missionname, string constellation_name, string sensor_name)
        {
            //add a new chain for the current sat
            IAgChain chainptr;
            chainptr = (IAgChain)m_oApplication.CurrentScenario.Children.New(AgESTKObjectType.eChain, missionname + "_Chain");

            //add the constellation of stations to the STK chain
            chainptr.Objects.Add("Constellation/"+ constellation_name);
            //add the current satilite/sensor to the chain
            chainptr.Objects.Add("Satellite/" + missionname + "/Sensor/" + sensor_name);
            return chainptr;

        }

        public void set_tdrs_enabled(bool x)
        {
            Tdrss_enabled = x;
            //if(x == true) Array.Resize<station_str>(ref stationdata, stationcount + 20);
        }

        public void set_startenddates(string start, string end)
        {
            startdate = start;
            enddate = end;
            //Console.Write("start/end=" + start + " " + end + "\n");
        }

        private void scenarioCheck()
        {
            try
            {
                if (this.m_oApplication.CurrentScenario == null)
                {//scenario doesn't exist
                    this.m_oApplication.CloseScenario();
                    this.m_oApplication.NewScenario("NPAS_Schedule_to_STK");
                }
            }catch
            {
                find_STK();

                if (this.m_oApplication.CurrentScenario == null)
                {//scenario doesn't exist
                    this.m_oApplication.CloseScenario();
                    this.m_oApplication.NewScenario("NPAS_Schedule_to_STK");
                }
            }
            IAgUnitPrefsDimCollection dimensions = this.m_oApplication.UnitPreferences;
            dimensions.ResetUnits();
            dimensions.SetCurrentUnit("DateFormat", "UTCG"); //DD/MM/YYYY
            //dimensions.SetCurrentUnit("DateFormat", "DD/MM/YYYY"); //DD/MM/YYYY
            IAgScenario scene = (IAgScenario)this.m_oApplication.CurrentScenario;

            //#TODO times should be set by the user
            DateTime start, stop;
            start = Convert.ToDateTime(startdate);
            stop = Convert.ToDateTime(enddate);

            Console.Write("start/end=" + start.ToString("dd MMM yyyy") + " 00:00:00.000 /" + stop.ToString("dd MMM yyyy") + " 00:00:00.000\n");
            try
            {
                scene.StopTime = stop.ToString("dd MMM yyyy");// + " 00:00:00";
                scene.StartTime = start.ToString(format: "dd MMM yyyy");// + " 00:00:00";
            }
            catch
            {
                scene.StartTime = start.ToString(format:"dd MMM yyyy");// + " 00:00:00";
                scene.StopTime = stop.ToString("dd MMM yyyy");// + " 00:00:00";

            }



            dimensions.SetCurrentUnit("DistanceUnit", "km");
            dimensions.SetCurrentUnit("TimeUnit", "sec");
            dimensions.SetCurrentUnit("AngleUnit", "deg");
            dimensions.SetCurrentUnit("MassUnit", "kg");
            dimensions.SetCurrentUnit("PowerUnit", "dbw");
            dimensions.SetCurrentUnit("FrequencyUnit", "ghz");
            dimensions.SetCurrentUnit("SmallDistanceUnit", "m");
            dimensions.SetCurrentUnit("latitudeUnit", "deg");
            dimensions.SetCurrentUnit("longitudeunit", "deg");
            dimensions.SetCurrentUnit("DurationUnit", "HMS");
            dimensions.SetCurrentUnit("Temperature", "K");
            dimensions.SetCurrentUnit("SmallTimeUnit", "sec");
            dimensions.SetCurrentUnit("RatioUnit", "db");
            dimensions.SetCurrentUnit("rcsUnit", "dbsm");
            dimensions.SetCurrentUnit("DopplerVelocityUnit", "m/s");
            dimensions.SetCurrentUnit("Percent", "unitValue");

        }



        private void AddWaypoint(IAgVeWaypointsCollection waypoints, object Lat, object Lon, double Alt, double Speed, double tr)
        {
            IAgVeWaypointsElement elem = waypoints.Add();
            elem.Latitude = Lat;
            elem.Longitude = Lon;
            elem.Altitude = Alt;
            elem.Speed = Speed;
            elem.TurnRadius = tr;
        }
        //process the checked missions in the checkbox list
        public void process_checklist(CheckedListBox passed, orbit_str[] data)
        {
            for (int i = 0; i < passed.Items.Count; i++)
            {
                //check if the current item is checked
                if (passed.GetItemChecked(i))
                    //look for the current check mission
                    for (int j = 0; j < orbitmissioncount; j++)
                        if (passed.Items[i].ToString() == data[j].name.ToString())
                        {
                            Console.Write("found selected mission " + data[j].name + "\n");
                            //mark for use into STK
                            data[j].used = 1;
                            if(data[j].misnum != data[j].cod_id)
                            {
                                for(int m=0;m<orbitmissioncount;m++)
                                    if(data[j].cod_id == data[m].misnum)
                                    {
                                        data[m].used = 1;
                                        break;
                                    }
                            }
                            break;
                        }
            }
        }
        //process the checked stations in the checkbox list
        public void process_checklist(CheckedListBox passed, station_str[] data)
        {
            for (int i = 0; i < passed.Items.Count; i++)
            {
                if (passed.GetItemChecked(i))
                    for (int j = 0; j < stationcount; j++)
                        if (passed.Items[i].ToString() == data[j].name.ToString())
                        {
                            Console.Write("found selected station " + data[j].name + "\n");
                            //mark for use into STK
                            data[j].used = true;
                            break;
                        }
            }
        }

        public void print_selected()
        {
            for (int j = 0; j < orbitmissioncount; j++)
                if (orbitdata[j].used == 1)
                    Console.Write("found mission=" + orbitdata[j].name + "\n");

        }

        //added selected TDRSs into the model
        private void add_tdrs(station_str passed)
        {
            IAgSatellite localtdrs;
            localtdrs = (IAgSatellite)m_oApplication.CurrentScenario.Children.New(AGI.STKObjects.AgESTKObjectType.eSatellite, passed.name );
            //AGI.STKObjects.IAgSatellite sat = (IAgSatellite)m_oApplication.CurrentScenario.Children.NewOnCentralBody(AGI.STKObjects.AgESTKObjectType.eSatellite, orbitdata[i].name, centerbodyname);
            //disable the leading ground track
            localtdrs.Graphics.PassData.GroundTrack.SetLeadDataType(AgELeadTrailData.eDataNone);
            //disable trailing ground track
            localtdrs.Graphics.PassData.GroundTrack.SetTrailDataType(AgELeadTrailData.eDataNone);
            localtdrs.VO.Pass.TrackData.PassData.GroundTrack.SetLeadDataType(AgELeadTrailData.eDataNone);
            localtdrs.VO.Pass.TrackData.PassData.GroundTrack.SetTrailDataType(AgELeadTrailData.eDataNone);

            localtdrs.VO.Pass.TrackData.PassData.Orbit.SetLeadDataType(AgELeadTrailData.eDataNone);
            localtdrs.VO.Pass.TrackData.PassData.Orbit.SetTrailDataType(AgELeadTrailData.eDataNone);

            //set the propagator type to HPOP
            localtdrs.SetPropagatorType(AGI.STKObjects.AgEVePropagatorType.ePropagatorJ2Perturbation);


            AGI.STKObjects.IAgVePropagatorJ2Perturbation hpop = (AGI.STKObjects.IAgVePropagatorJ2Perturbation)localtdrs.Propagator;

            IAgOrbitState orbit = hpop.InitialState.Representation;
            //create the string to hold the missions epoch date & time
            string cmb_epoch = startdate, cmb_end = enddate;
            cmb_epoch += " 00:00:00";
            DateTime epochDT = Convert.ToDateTime(cmb_epoch);
            DateTime endepochDT = Convert.ToDateTime(cmb_end);


            //hpop.InitialState.Representation.Assign(orbit);
            Console.Write("epochtime = " + epochDT.ToString("dd MMM yyyy ") + "00:00:00"  + "\n");
            hpop.InitialState.Representation.Epoch = (epochDT.ToString("dd MMM yyyy ") + "00:00:00");

           // hpop.InitialState.Representation.Assign(ia)
            //hpop.InitialState.Representation.AssignClassical(AgECoordinateSystem.eCoordinateSystemJ2000, orbitdata[i].sma, orbitdata[i].ecc, orbitdata[i].inc, orbitdata[i].aop, orbitdata[i].raan, orbitdata[i].ma);
            //hpop.Propagate();


            IAgOrbitStateClassical classical = (IAgOrbitStateClassical)hpop.InitialState.Representation.ConvertTo(AgEOrbitStateType.eOrbitStateClassical);
            classical.CoordinateSystemType = AGI.STKUtil.AgECoordinateSystem.eCoordinateSystemJ2000;
            IAgCrdnEventIntervalSmartInterval interval = hpop.EphemerisInterval;
            interval.SetExplicitInterval((epochDT.ToString("dd MMM yyyy ") + "00:00:00"), (endepochDT.ToString("dd MMM yyyy ") + "00:00:00"));

            hpop.Step = 60;
            classical.LocationType = AgEClassicalLocation.eLocationTrueAnomaly;
            IAgClassicalLocationTrueAnomaly trueAnomaly = (IAgClassicalLocationTrueAnomaly)classical.Location;
            trueAnomaly.Value = 178.845262;

            classical.SizeShapeType = AgEClassicalSizeShape.eSizeShapePeriod;
            IAgClassicalSizeShapePeriod period = (IAgClassicalSizeShapePeriod)classical.SizeShape;
            period.Eccentricity = 0.0;

            period.Period = 86164.090540;

            classical.Orientation.ArgOfPerigee = 0.0;

            classical.Orientation.Inclination = 0.0;

            classical.Orientation.AscNodeType = AgEOrientationAscNode.eAscNodeLAN;
            IAgOrientationAscNodeLAN lan = (IAgOrientationAscNodeLAN)classical.Orientation.AscNode;
            lan.Value = (360 - passed.lon );

            hpop.InitialState.Representation.Assign(classical);

            hpop.Propagate();


        }

        private void groundtrack_set(IAgSatellite passedsat, bool enabled)
        {
            if (enabled == false)
            {
                passedsat.Graphics.PassData.GroundTrack.SetLeadDataType(AgELeadTrailData.eDataNone);
                //disable trailing ground track
                passedsat.Graphics.PassData.GroundTrack.SetTrailDataType(AgELeadTrailData.eDataNone);
                passedsat.VO.Pass.TrackData.PassData.GroundTrack.SetLeadDataType(AgELeadTrailData.eDataNone);
                passedsat.VO.Pass.TrackData.PassData.GroundTrack.SetTrailDataType(AgELeadTrailData.eDataNone);

                passedsat.VO.Pass.TrackData.PassData.Orbit.SetLeadDataType(AgELeadTrailData.eDataNone);
                passedsat.VO.Pass.TrackData.PassData.Orbit.SetTrailDataType(AgELeadTrailData.eDataNone);
            }
        }
        private void groundtrack_set(IAgSatellite passedsat, AgELeadTrailData taildata)
        {

            passedsat.Graphics.PassData.GroundTrack.SetLeadDataType(taildata);
            //disable trailing ground track
            passedsat.Graphics.PassData.GroundTrack.SetTrailDataType(taildata);
            //VO is the 3D window???
            passedsat.VO.Pass.TrackData.PassData.GroundTrack.SetLeadDataType(taildata);
            passedsat.VO.Pass.TrackData.PassData.GroundTrack.SetTrailDataType(taildata);
            passedsat.VO.Pass.TrackData.PassData.Orbit.SetLeadDataType(taildata);
            passedsat.VO.Pass.TrackData.PassData.Orbit.SetTrailDataType(taildata);

        }

        public void Orbit_generation()
        {

            //load_orbit_file();
            scenarioCheck();
            var format = new NumberFormatInfo();
            format.NegativeSign = "-";
            format.NumberDecimalSeparator = ".";
            // planetodetic.Lat = Double.Parse(lat2[i], format);

            for (int i = 0; i < orbitmissioncount; i++)
            {
                //debug infomation///////
                if (orbitdata[i].misnum == orbitdata[i].cod_id && orbitdata[i].used == 1 && orbitdata[i].efileused == false)
                {
                    //generate the timeline file
                    TL_file_generator(orbitdata[i].name, orbitdata[i].name);

                    Console.Write("Misnum == cod_id\n");

                    Console.Write("Mission= " + orbitdata[i].name + "\n" + orbitdata[i].epoch + " " + orbitdata[i].epoch_time + " ");
                    Console.Write(orbitdata[i].sma + " " + orbitdata[i].ecc + " " + orbitdata[i].inc + " " + orbitdata[i].raan + " " + orbitdata[i].aop + " " + orbitdata[i].ma + "\n");

                    string centerbodyname;
                    if (orbitdata[i].centerbody == 1)
                    {
                        centerbodyname = "Earth";
                    }
                    else if (orbitdata[i].centerbody == 2)
                    {
                        centerbodyname = "Moon";
                    }
                    else centerbodyname = "Earth";


                    // AGI.STKObjects.IAgSatellite sat = (IAgSatellite)m_oApplication.CurrentScenario.Children.New(AGI.STKObjects.AgESTKObjectType.eSatellite, mission2[i]);
                    //sat.SetPropagatorType(AGI.STKObjects.AgEVePropagatorType.ePropagatorTwoBody);

                    try
                    {
                        //create a new sat with the cod orbit details;
                        orbitdata[i].MisSat = (IAgSatellite)m_oApplication.CurrentScenario.Children.NewOnCentralBody(AGI.STKObjects.AgESTKObjectType.eSatellite, orbitdata[i].name, centerbodyname);
                        //AGI.STKObjects.IAgSatellite sat = (IAgSatellite)m_oApplication.CurrentScenario.Children.NewOnCentralBody(AGI.STKObjects.AgESTKObjectType.eSatellite, orbitdata[i].name, centerbodyname);
                    }catch
                    {
                        //sat already exists reset the scenario;
                        this.m_oApplication.CloseScenario();
                        scenarioCheck();
                        Orbit_generation();
                        return;

                    }
                    //disable the leading ground track
                    groundtrack_set(orbitdata[i].MisSat, groundtrack_displayed);

                    //set the propagator type to HPOP
                    orbitdata[i].MisSat.SetPropagatorType(AGI.STKObjects.AgEVePropagatorType.ePropagatorHPOP);

                    //TODO update below code to use sat not hpop;
                    AGI.STKObjects.IAgVePropagatorHPOP hpop = (AGI.STKObjects.IAgVePropagatorHPOP)orbitdata[i].MisSat.Propagator;

                    IAgOrbitState orbit = hpop.InitialState.Representation;
                    //create the string to hold the missions epoch date & time
                    string cmb_epoch = orbitdata[i].epoch;
                    cmb_epoch += " ";
                    cmb_epoch += orbitdata[i].epoch_time;
                    DateTime epochDT = Convert.ToDateTime(orbitdata[i].epoch);
                    DateTime startDT = Convert.ToDateTime(orbitdata[i].start_date);
                    DateTime endDT;

                    if( orbitdata[i].endopt == 0 )
                    {
                        endDT = startDT;
                        endDT = endDT.AddDays((double)orbitdata[i].duration);
                        orbitdata[i].end_time = orbitdata[i].start_time;
                    }
                    else
                    {
                        endDT = Convert.ToDateTime(orbitdata[i].end_date);
                    }

                    //hpop.ForceModel.EclipsingBodies.AssignEclipsingBody( centerbodyname);
                    //check the centerbody of the provided orbit data;
                    if (orbitdata[i].centerbody == 1)
                    {// centerbody == EARTH
                        Console.Write("centerbody2[i]) == 1 \n");
                        Console.Write("\n Centralbodyfile=" + hpop.ForceModel.CentralBodyGravity.File + "\n");

                        hpop.ForceModel.Drag.Use = false;
                        hpop.ForceModel.SolarRadiationPressure.Use = false;
                    }
                    else if (orbitdata[i].centerbody == 2)
                    {//CenterBody == Moon
                        Console.Write("centerbody2[i]) == 2 \n");
                        hpop.ForceModel.Drag.Use = false;
                        hpop.ForceModel.SolarRadiationPressure.Use = false;

                        Console.Write("\n Centralbodyfile=" + hpop.ForceModel.CentralBodyGravity.File + "\n");
                        //change gravity file for HPOP use with the moon
                        hpop.ForceModel.CentralBodyGravity.File = "STKData\\CentralBodies\\Moon\\LP100K.grv";

                    }
                    //hpop.InitialState.Representation.Assign(orbit);
                    Console.Write("set epochtime = " + epochDT.ToString("dd MMM yyyy ") + orbitdata[i].epoch_time + "\n");
                    //set the epoch date/time from the orbit bin file
                    hpop.InitialState.Representation.Epoch = ( epochDT.ToString("dd MMM yyyy ") + orbitdata[i].epoch_time );

                    Console.Write("set start/stop times = " + (startDT.ToString("dd MMM yyyy ") + orbitdata[i].start_time) + " / " + startDT.AddDays(1).ToString("dd MMM yyyy ") + "\n");
                    //only proagate the orbit 1 day from the start date/time to get the required data to convert to a Fixed coordnate system
                    hpop.EphemerisInterval.SetStartAndStopTimes((startDT.ToString("dd MMM yyyy ") + orbitdata[i].start_time), startDT.AddDays(1).ToString("dd MMM yyyy "));
                    //hpop.EphemerisInterval.SetStartAndStopTimes((startDT.ToString("dd MMM yyyy ") + orbitdata[i].start_time), (endDT.ToString("dd MMM yyyy ") + orbitdata[i].end_time));
                    hpop.InitialState.Representation.AssignClassical(AgECoordinateSystem.eCoordinateSystemJ2000, orbitdata[i].sma , orbitdata[i].ecc, orbitdata[i].inc, orbitdata[i].aop, orbitdata[i].raan, orbitdata[i].ma);

                    hpop.Propagate();
                    //check if the orbit is earth centered and that the
                    //     orbit start date is different from the Mission model date
                    if (orbitdata[i].centerbody == 1 && orbitdata[i].start_date != startdate)
                    {
                        Console.Write("Startdate != orbit_start date converting to Cartesian elements for fixed system" + "\n");

                        IAgStkObject sat = m_oApplication.CurrentScenario.Children[orbitdata[i].name];

                        // Get the satellite's ICRF cartesian position at 180 EpSec using the data provider interface
                        IAgDataProviderGroup dpGroup = sat.DataProviders["Cartesian Position"] as IAgDataProviderGroup;
                        Array elements = new object[] { "x", "y", "z" };
                        //***TODO*** find J2000 group instead of ICRF
                        IAgDataPrvTimeVar dp = dpGroup.Group["ICRF"] as IAgDataPrvTimeVar;

                        //get the elements at the start date/time of the orbit using in NPAS
                        IAgDrResult dpResult = dp.ExecSingleElements(hpop.StartTime, ref elements);

                        Console.Write("hpop.startTime=" + hpop.StartTime + "\n");


                        double xICRF = (double)dpResult.DataSets[0].GetValues().GetValue(0);
                        double yICRF = (double)dpResult.DataSets[1].GetValues().GetValue(0);
                        double zICRF = (double)dpResult.DataSets[2].GetValues().GetValue(0);

                        // Get the satellite's ICRF cartesian velocity at 180 EpSec using the data provider interface
                        dpGroup = sat.DataProviders["Cartesian Velocity"] as IAgDataProviderGroup;

                        //***TODO*** find J2000 group instead of ICRF
                        dp = dpGroup.Group["ICRF"] as IAgDataPrvTimeVar;

                        //get the elements at the start date/time of the orbit using in NPAS
                        dpResult = dp.ExecSingleElements(hpop.StartTime, ref elements);

                        double xvelICRF = (double)dpResult.DataSets[0].GetValues().GetValue(0);
                        double yvelICRF = (double)dpResult.DataSets[1].GetValues().GetValue(0);
                        double zvelICRF = (double)dpResult.DataSets[2].GetValues().GetValue(0);

                        Console.Write("J2000 cartesian vectors\n");
                        Console.Write("X=" + xICRF + " Y=" + yICRF + " Z=" + zICRF + "\n");
                        Console.Write("Xd=" + xvelICRF + " Yd=" + yvelICRF + " Zd=" + zvelICRF + "\n");

                        // Create a position vector using the ICRF coordinates
                        IAgCrdnAxes axesICRF = sat.Vgt.WellKnownAxes.Earth.ICRF;
                        IAgCartesian3Vector vectorICRF = m_oApplication.ConversionUtility.NewCartesian3Vector();

                        vectorICRF.Set(xICRF, yICRF, zICRF);

                        // Create a velocity vector using the ICRF coordinates
                        IAgCartesian3Vector vectorvelICRF = m_oApplication.ConversionUtility.NewCartesian3Vector();
                        vectorvelICRF.Set(xvelICRF, yvelICRF, zvelICRF);


                        // Use the TransformWithRate method to transform ICRF to Fixed
                        IAgCrdnAxes axesFixed = sat.Vgt.WellKnownAxes.Earth.Fixed;
                        IAgCrdnAxesTransformWithRateResult result = axesICRF.TransformWithRate(hpop.StartTime, axesFixed, vectorICRF, vectorvelICRF);

                        // Get the Fixed position and velocity coordinates
                        double xFixed = result.Vector.X;
                        double yFixed = result.Vector.Y;
                        double zFixed = result.Vector.Z;
                        double xvelFixed = result.Velocity.X;
                        double yvelFixed = result.Velocity.Y;
                        double zvelFixed = result.Velocity.Z;

                        Console.Write("converted cartesian vectors" + "\n");
                        Console.Write("X=" + xFixed + " Y=" + yFixed + " Z=" + zFixed + "\n");
                        Console.Write("Xd=" + xvelFixed + " Yd=" + yvelFixed + " Zd=" + zvelFixed + "\n");

                        DateTime start, stop;
                        start = Convert.ToDateTime(startdate);
                        stop = Convert.ToDateTime(enddate);

                        //set the epoch and start date/time to the selected scenario date/time
                        hpop.InitialState.Representation.Epoch = (start.ToString("dd MMM yyyy "));
                        hpop.EphemerisInterval.SetStartAndStopTimes((start.ToString("dd MMM yyyy ")), (stop.ToString("dd MMM yyyy ")));

                        hpop.InitialState.Representation.AssignCartesian(AgECoordinateSystem.eCoordinateSystemFixed, xFixed, yFixed, zFixed, xvelFixed, yvelFixed, zvelFixed);

                        hpop.InitialState.Representation.Epoch = (start.ToString("dd MMM yyyy "));
                        hpop.EphemerisInterval.SetStartAndStopTimes((start.ToString("dd MMM yyyy ")), (stop.ToString("dd MMM yyyy ")));

                        //propagate new orbit
                        hpop.Propagate();

                    }

                    orbitdata[i].Missensor = generate_sensor(orbitdata[i].name, "Stations");
                    orbitdata[i].MisChain = generate_chain(orbitdata[i].name, "Stations", orbitdata[i].name + "_sensor");

                }
                else if (orbitdata[i].used == 1 && orbitdata[i].efileused == false)
                {
                    //generate the missions timeline file inside the orignial cod mis name
                    //don't generate multiple orbits on top of each other;
                    TL_file_generator(orbitdata[i].name, missionindex[orbitdata[i].cod_id]);
                    orbitdata[i].MisChain = null;
                    Console.Write("Misnum != cod_id && orbitdata[i].used == 1 for " + orbitdata[i].name + "\n");

                }else if ( orbitdata[i].used == 1 && orbitdata[i].efileused == true)
                {
                    TL_file_generator(orbitdata[i].name, missionindex[orbitdata[i].cod_id]);

                    orbitdata[i].MisSat = (IAgSatellite)m_oApplication.CurrentScenario.Children.New(AGI.STKObjects.AgESTKObjectType.eSatellite, orbitdata[i].name);
                    //disable the leading ground track
                    groundtrack_set(orbitdata[i].MisSat, groundtrack_displayed);

                    //set the propagator type to STK E file
                    orbitdata[i].MisSat.SetPropagatorType(AGI.STKObjects.AgEVePropagatorType.ePropagatorStkExternal);

                    AGI.STKObjects.IAgVePropagatorStkExternal EFileProp = (AGI.STKObjects.IAgVePropagatorStkExternal)orbitdata[i].MisSat.Propagator;

                    //strip the file name from the entire director name;
                    EFileProp.Filename = (Efile_directory + orbitdata[i].efilename.Split('/').Last().ToString());
                    //propagate the efile orbit
                    EFileProp.Propagate();

                    //add new sensor to the current sat and change the sensor type to target
                    orbitdata[i].Missensor = generate_sensor(orbitdata[i].name, "Stations");

                    //add a new chain for the current sat
                    orbitdata[i].MisChain = generate_chain(orbitdata[i].name, "Stations", orbitdata[i].name + "_sensor");

                }
                else
                {
                    orbitdata[i].MisChain = null;
                    Console.Write("Misnum != cod_id for " + orbitdata[i].name + "\n");
                }
            }
        }


        public void Station_generation()
        {

            //load_Station_File();
            scenarioCheck();
            IAgConstellation localconstellation;
            try
            {
                localconstellation = (IAgConstellation)m_oApplication.CurrentScenario.Children.New(AgESTKObjectType.eConstellation, "Stations");
            }
            catch
            {
                if (failure == false)
                {
                    failure = true;
                    //failed to add constellation close the currently opened scenario and attempt to open a new scenario;
                    this.m_oApplication.CloseScenario();
                    this.Station_generation();
                }
                return;
            }

            //currently there is no way to set the 2d graphics properties for the scenario listed in the tutorial
            for (int i = 0; i < stationcount; i++)
            {
                if (stationdata[i].used == true)
                {
                    if (stationdata[i].tdrs == false)
                    {
                        //debug infomation///////
                        Console.Write(stationdata[i].name + " " + stationdata[i].lat + " " + stationdata[i].lon + " " + stationdata[i].altitude + "\n");
                        ////////////////////////////
                        IAgFacility agistationdata = (IAgFacility)m_oApplication.CurrentScenario.Children.New(AgESTKObjectType.eFacility, stationdata[i].name);
                        agistationdata.UseTerrain = false;
                        IAgPlanetodetic planetodetic = (IAgPlanetodetic)agistationdata.Position.ConvertTo(AgEPositionType.ePlanetodetic);
                        //set lat lon and hight of ground station;
                        planetodetic.Lat = stationdata[i].lat;
                        planetodetic.Lon = stationdata[i].lon;
                        planetodetic.Alt = stationdata[i].altitude;
                        agistationdata.Position.Assign(planetodetic);
                        Random r = new Random();
                        //randomly generate colors for each station
                        agistationdata.Graphics.Color = Color.FromArgb(r.Next(10000000, 99999999));



                        string localobjectname = "Facility/" + stationdata[i].name;
                        Console.Write(" localobjectname=" + localobjectname + "\n");

                        localconstellation.Objects.Add(localobjectname);
                    }
                    else
                    {
                        add_tdrs(stationdata[i]);
                        string localobjectname = "Satellite/" + stationdata[i].name;
                        localconstellation.Objects.Add(localobjectname);
                    }
                }
            }

            //((IAgStkObject)perth).ShortDescription = "Australian Tracking Station";

            //			IAgFacility santiago = (IAgFacility)m_oApplication.CurrentScenario.Children["Santiago"];
            //baikonur.Graphics.Color = Color.Black;
            //perth.Graphics.Color = Color.FromArgb(16777215);

            ((IAgAnimation)this.m_oApplication).Rewind();

            //Console.Write("Press Enter to exit application:");
            //Console.ReadLine();
        }

        //generate timeline files for the passed mission name in the codname file
        public void TL_file_generator(string misname, string codname)
        {
            string filename;
            bool fileexist = false;
            //create the interval file name in the tmp dir with the cod mission name + .int
            filename = Tmp_directory + codname + ".int";
            //check if the file already exists
            fileexist = File.Exists(filename);
            StreamWriter write = File.AppendText(filename);
            //if the file didn't already exists then write the STK interval file header
            if(fileexist == false)
            {
                write.Write("stk.v.11.0.0 \n\n BEGIN IntervalList \n\n \t\t\tDATEUNITABRV UTCG \n\n BEGIN Intervals \n");
            }
            //loop though timeline data to look for the provided missions access times
            for(int i=0;i<tlcount;i++)
            {
                if(tldata[i].mis == misname)
                {
                    write.Write("\"" + Convert.ToDateTime(tldata[i].aosdate).ToString("dd MMM yyyy") + " " + tldata[i].aostime + "\" \"" + Convert.ToDateTime(tldata[i].losdate).ToString("dd MMM yyyy") + " " + tldata[i].lostime);
                    bool a = Array.Exists<station_str>(stationdata, x => x.name == tldata[i].sta && x.tdrs == true);

                    if (a == false)
                    {
                        write.Write("\" Facility/" + tldata[i].sta + "\n");
                    }else
                    {
                        write.Write("\" Satellite/" + tldata[i].sta + "\n");
                    }
                }
            }
            //close the opened timeline file
            write.Close();
        }

        public void Set_intervalFiles()
        {
            for (int i = 0; i < orbitmissioncount; i++)
            {
                //debug infomation///////
                if (orbitdata[i].misnum == orbitdata[i].cod_id && orbitdata[i].used == 1)
                {
                    try
                    {
                        Console.Write(" interval_file=" + Tmp_directory + orbitdata[i].name + ".int\n");
                        //orbitdata[i].MisChain.SetAccessIntervalsFile( Tmp_directory + orbitdata[i].name + ".int");
                        // orbitdata[i].MisChain.Constraints.UseLoadIntervalFile = true;
                        // orbitdata[i].MisChain.Constraints.LoadIntervalFile = (Tmp_directory + orbitdata[i].name + ".int");

                        IAgAccessCnstrIntervals sattest = orbitdata[i].Missensor.AccessConstraints.AddConstraint(AgEAccessConstraints.eCstrIntervals) as IAgAccessCnstrIntervals;
                        // orbitdata[i].Missensor.AccessConstraints
                        sattest.Filename = (Tmp_directory + orbitdata[i].name + ".int");
                    }catch
                    {
                        Console.Write(" Warning interval file not found");
                        MessageBox.Show("Warning interval file not found for mission " + orbitdata[i].name);
                    }
                }
            }
        }

    }//end of class

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NPAS_to_STK());
        }
    }
}
