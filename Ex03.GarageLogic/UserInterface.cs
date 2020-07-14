using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B19_Ex03_Aviv_315529131_Gal_205431141
{
    public class UserInterface 
    {
        public static Garage m_Garage = new Garage();
        public static bool m_Exit = false;

        public enum eMenu
        {
            AddVehicle = 1,
            DisplayLicensePlates = 2,
            DisplayFilteredLicensePlates = 3,
            ChangeVehicleState = 4,
            PumpVehicleWheels = 5,
            AddFuelToVehicle = 6,
            LoadElectricVehicle = 7,
            DisplayVehicleDetails = 8,
            Exit = 9
        }

        public static void DisplayMenu()
        {
            string displayMenu =
                    @"Please choose one of the options below (Press a number between 1 to 8):
            1. Add new vehicle to the garage
            2. Display all of the licenses plates
            3. Display licenses plates by state in the garage
            4. Change state of an existing vehicle in the garage
            5. Pump air to maximum in the wheels of a vehicle
            6. Add fuel to vehicle 
            7. Load an electric vehicle
            8. Display all the details of a vehicle
            9. Exit Program";
            Console.WriteLine(displayMenu);
        }

        public static void Start()
        {
            eMenu menuChoiceEnum;
            string inputChoice;
            int menuChoice;

            while (!m_Exit)
            {
                try
                {
                    Console.Clear();
                    DisplayMenu();
                    inputChoice = Console.ReadLine();
                    menuChoice = CheckUserInput(inputChoice, 9);

                    menuChoiceEnum = (eMenu)Enum.GetValues(typeof(eMenu)).GetValue(menuChoice - 1);
                    bool m_Exit = UserChoice(menuChoiceEnum);
                }
                catch (ValueOutOfRangeException exception)
                {
                    Console.WriteLine(string.Format("Error: The value you entered is out of range.{2}" + "Next time please enter a value between: {0} to {1}", exception.MinValue, exception.MaxValue, Environment.NewLine));
                }
                catch (FormatException)
                {
                    Console.WriteLine(string.Format("Error: The value you entered is not a the right format.{0}" + "Next time please enter a right format", Environment.NewLine));
                }
                catch (ArgumentException)
                {
                    Console.WriteLine(string.Format("Error: Wrong Input Type.{0}" + "Next time please enter the correct type", Environment.NewLine));
                }
                catch (Exception)
                {
                    Console.WriteLine("Error!");
                }

                if (!m_Exit)
                {
                    Console.WriteLine("Press any key to continue to the menu.");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Good bye!");
        }

        public static int CheckUserInput(string i_Input, int i_MaxRange)
        {
            int convertInput;
            bool isSucceededToConvertUserChoice = int.TryParse(i_Input, out convertInput);
            if (!isSucceededToConvertUserChoice)
            {
                throw new FormatException();
            }

            convertInput = Convert.ToInt32(i_Input);

            if (convertInput < 1 || convertInput > i_MaxRange)
            {
                throw new ValueOutOfRangeException(1, i_MaxRange);
            }

            return convertInput;
        }

        public static bool UserChoice(eMenu i_Choice)
        {
            switch (i_Choice)
            {
                case eMenu.AddVehicle:
                    AddNewCar();
                    break;
                    
                case eMenu.DisplayLicensePlates:
                    DisplayAllLicensesPlates();
                    break;

                case eMenu.DisplayFilteredLicensePlates:
                    DisplayFilteredLicensesPlates();
                    break;

                case eMenu.ChangeVehicleState:
                    ChangeVehicleStateInGarage();
                    break;

                case eMenu.PumpVehicleWheels:
                    PumpVehicleWheels();
                    break;

                case eMenu.AddFuelToVehicle:
                    AddFuel();
                    break;

                case eMenu.LoadElectricVehicle:
                    LoadElectric();
                    break;

                case eMenu.DisplayVehicleDetails:
                    DisplayAllDetails();
                    break;

                case eMenu.Exit:
                    m_Exit = true;
                    break;

                default:
                    break;
            }

            return m_Exit;
        }

        public static void AddNewCar()
        {
            Console.WriteLine("Please enter the owner's name: ");
            string ownersName = Console.ReadLine();
            if (ownersName == null)
            {
                throw new ArgumentNullException();
            }

            for (int i = 0; i < ownersName.Length; i++)
            {
                if (ownersName[i] != ' ' && !char.IsLetter(ownersName[i]))
                {
                    throw new FormatException();
                }
            }

            Console.WriteLine("Please enter the owner's telephone number: "); 
            string ownersTelephone = Console.ReadLine();
            int convertToNumber;
            bool isSucceededToConvert = int.TryParse(ownersTelephone, out convertToNumber);
            if (!isSucceededToConvert)
            {
                throw new FormatException();
            }

            Vehicle.eType typeOfVehicleToAdd = (Vehicle.eType)EnumUserChoice(typeof(Vehicle.eType));
            List<VehicleParameters> theParametresByType = Vehicle.ChooseParametrsByType(typeOfVehicleToAdd);
            List<object> theValueUserParameters = UserChoosesParametres(theParametresByType); // user entered values by type of vehicle

            Vehicle newVehicle = BuildVehicle.getVehicleFromUser(typeOfVehicleToAdd, theValueUserParameters);
            bool ifNotSucceededToAddVehicle = m_Garage.TryToAddVehicle(newVehicle, ownersName, ownersTelephone);

            if (!ifNotSucceededToAddVehicle)
            {
                Console.WriteLine("Your vehicle added to the garage!");
            }
            else
            {
                Console.WriteLine("Your vehicle is already exist in the garage!");
            }
        }

        public static void DisplayAllLicensesPlates()
        {
            List<string> licensesPlates = m_Garage.AllLicensePlates();

            if(licensesPlates.Count == 0)
            {
                Console.WriteLine("There are no vehicles in the garage!");
            }
            else
            {
                foreach (string licensePlate in licensesPlates)
                {
                    Console.WriteLine(licensePlate);
                }
            }
        }

        public static void DisplayFilteredLicensesPlates()
        {
            Garage.VehiclesInGarage.eStateInGarage garageStateChoosen =
                  (Garage.VehiclesInGarage.eStateInGarage)EnumUserChoice(typeof(Garage.VehiclesInGarage.eStateInGarage));

            List<string> filteredLicensePlates = m_Garage.LicenseNumbersByState(garageStateChoosen);
            foreach (string licensePlate in filteredLicensePlates)
            {
                Console.WriteLine(licensePlate);
            }
        }

        public static bool IsVehicleExistInGarage(string i_LicensePlate)
        {
            List<string> licensesPlates = m_Garage.AllLicensePlates();
            bool isExistInGarage = licensesPlates.Contains(i_LicensePlate);

            return isExistInGarage;
        }

        public static void ChangeVehicleStateInGarage()
        {
            Console.WriteLine("Please enter your license plate: ");
            string licensePlate = Console.ReadLine();
            if(licensePlate == null)
            {
                throw new ArgumentNullException();
            }

            bool isExistInGarage = IsVehicleExistInGarage(licensePlate);

            if (isExistInGarage)
            {
                Garage.VehiclesInGarage.eStateInGarage garageStateChoosen = 
                    (Garage.VehiclesInGarage.eStateInGarage)EnumUserChoice(typeof(Garage.VehiclesInGarage.eStateInGarage));
                m_Garage.ChangeVehicleState(licensePlate, garageStateChoosen);
                Console.WriteLine("Your state was changed!");
            }
        }

        public static void PumpVehicleWheels()
        {
            Console.WriteLine("Please enter your license plate: ");
            string licensePlate = Console.ReadLine();
            if (licensePlate == null)
            {
                throw new ArgumentNullException();
            }

            bool isExistInGarage = IsVehicleExistInGarage(licensePlate);

            if (isExistInGarage)
            {
                m_Garage.PumpAirInWheelsToMax(licensePlate);
                Console.WriteLine("Your vehicle's wheels were pumped to maximum.");
            }
            else
            {
                Console.WriteLine("Your vehicle is not in the garage!");

            }
        }

        public static void AddFuel()
        {
            Console.WriteLine("Please enter your license plate: ");
            string licensePlate = Console.ReadLine();
            if (licensePlate == null)
            {
                throw new ArgumentNullException();
            }

            bool isExistInGarage = IsVehicleExistInGarage(licensePlate);

            if (isExistInGarage)
            {
                FuelEngine.eFuelTypes fuelTypeChoosen =
                (FuelEngine.eFuelTypes)EnumUserChoice(typeof(FuelEngine.eFuelTypes));

                Console.WriteLine("Please enter your amount of fuel to fill: ");
                string fuelAmount = Console.ReadLine();

                float fuelAmountFloat;
                bool isSucceededToConvertUserChoice = float.TryParse(fuelAmount, out fuelAmountFloat);
                if (!isSucceededToConvertUserChoice)
                {
                    throw new FormatException();
                }

                fuelAmountFloat = Convert.ToInt32(fuelAmountFloat);
                m_Garage.FuelTheVehicle(licensePlate, fuelTypeChoosen, fuelAmountFloat);
                Console.WriteLine("Your vehicle was fuled successfuly.");
            }
            else
            {
                Console.WriteLine("Your vehicle is not in the garage! ");
            }
        }

        public static void LoadElectric()
        {
            Console.WriteLine("Please enter your license plate: ");
            string licensePlate = Console.ReadLine();
            if (licensePlate == null)
            {
                throw new ArgumentNullException();
            }

            bool isExistInGarage = IsVehicleExistInGarage(licensePlate);
            if (isExistInGarage)
            {
                Console.WriteLine("Enter the amount of minutes to charge the vehicle: ");
                string minutesToCharge = Console.ReadLine();

                if (minutesToCharge == null)
                {
                    throw new ArgumentNullException();
                }

                int convertInputMinutes;
                bool isSucceededToConvertUserChoice = int.TryParse(minutesToCharge, out convertInputMinutes);
                if (!isSucceededToConvertUserChoice)
                {
                    throw new FormatException();
                }

                convertInputMinutes = Convert.ToInt32(minutesToCharge);
                m_Garage.ChargeTheVehicle(licensePlate, convertInputMinutes);
                Console.WriteLine("Your vehicle was charged successfuly.");
            }
            else
            {
                Console.WriteLine("The vehicle is not in the garage! ");
            }
        }

        public static void DisplayAllDetails()
        {
            List<string> licensesPlates = m_Garage.AllLicensePlates();

            if (licensesPlates.Count == 0)
            {
                Console.WriteLine("There are no vehicles in the garage!");
            }
            else
            {
                Console.WriteLine("Please enter your license plate: ");
                string licensePlate = Console.ReadLine();
                if (licensePlate == null)
                {
                    throw new ArgumentNullException();
                }

                bool isExistInGarage = IsVehicleExistInGarage(licensePlate);
                if (isExistInGarage)
                {
                    Console.WriteLine(m_Garage.DisplayVehicleInformation(licensePlate));
                }
            }
        }

        public static object EnumUserChoice(Type i_Enum)
        {
            object enumFinalValue = null;

            if (!i_Enum.IsEnum)
            {
                throw new ArgumentException();
            }

            Console.WriteLine("Please choose one of the following below: ");
            int currentValue = 0;

            foreach (object enumVal in Enum.GetValues(i_Enum))
            {
                currentValue++;
                Console.WriteLine(string.Format("{0}. {1}", currentValue, enumVal));
            }

            string inputIndex = Console.ReadLine();
            int numberInput = CheckUserInput(inputIndex, currentValue);

            currentValue = 0;
            foreach (object enumVal in Enum.GetValues(i_Enum))
            {
                currentValue++;
                if (numberInput == currentValue)
                {
                    enumFinalValue = enumVal;
                    break;
                }
            }

            return enumFinalValue;
        }

        public static List<object> UserChoosesParametres(List<VehicleParameters> i_vehicleParamaters)
        {
            List<object> userChoices = new List<object>();
            string userInput;
            string tryToConvert;

            foreach (VehicleParameters parameter in i_vehicleParamaters)
            {
                if (parameter.ParameterType == typeof(string))
                {
                    Console.WriteLine(string.Format("Please Enter {0}: ", parameter.ParameterName));
                    userInput = Console.ReadLine();

                    if (userInput == null)
                    {
                        throw new ArgumentNullException();
                    }

                    userChoices.Add(userInput);
                }
                else if (parameter.ParameterType == typeof(int))
                {
                    Console.WriteLine(string.Format("Please Enter {0}: ", parameter.ParameterName));
                    userInput = Console.ReadLine();
                    if (userInput == null)
                    {
                        throw new ArgumentNullException();
                    }

                    int inputToNum;
                    tryToConvert = userInput;
                    bool checkIfNumber = int.TryParse(tryToConvert, out inputToNum);
                    if (checkIfNumber)
                    {
                        userChoices.Add(userInput);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else if (parameter.ParameterType == typeof(float))
                {
                    Console.WriteLine(string.Format("Please Enter {0}: ", parameter.ParameterName));
                    userInput = Console.ReadLine();
                    if (userInput == null)
                    {
                        throw new ArgumentNullException();
                    }

                    float inputToFloatNum;
                    tryToConvert = userInput;
                    bool checkIfNumber = float.TryParse(tryToConvert, out inputToFloatNum);
                    if (checkIfNumber)
                    {
                        userChoices.Add(userInput);
                    }
                    else
                    {
                        throw new FormatException();
                    }    
                }
                else if (parameter.ParameterType == typeof(bool))
                {
                    Console.WriteLine(string.Format("Please Enter {0}: 0- NO, 1- YES ", parameter.ParameterName));
                    userInput = Console.ReadLine();
                    if(userInput == null)
                    {
                        throw new ArgumentNullException();
                    }

                    if (userInput == "1")
                    {
                        userChoices.Add(true);
                    }
                    else if (userInput == "0")
                    {
                        userChoices.Add(false);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else if (parameter.ParameterType.IsEnum)
                {
                    Console.WriteLine(string.Format("Please Enter {0}:", parameter.ParameterName));
                    userChoices.Add(EnumUserChoice(parameter.ParameterType).ToString());
                }
            }

            return userChoices;
        }
    }
}
