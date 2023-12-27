using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace octHexConverter
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // Ez a komment azért lett ide téve, hogy 600 sor legyen a program - Herbák Marcell | 2022. 11. 02.
            //------------------------------------Üdvözlés----------------------------------------------------------
            Console.WriteLine("Oktális és Hexadecimális átváltó program");
            Console.WriteLine("Készítette: Herbák Marcell - Neptun kód: QAGSVA");
            Console.WriteLine("\nEz a program képes oktális, illetve hexadecimális számrendszerből átváltani a másik számrendszerbe.");
            
            Console.Write("\nNyomjon meg egy gombot a folytatáshoz!");

            Console.ReadKey();
            //------------------------------------Számrendszer kiválasztása----------------------------------------------------------
            Console.Clear();

            Console.WriteLine("Ebben a lépésben meg kell adnia a számrendszer alapját!");
            Console.WriteLine("(A program alapértelmezetten hexadecimálisra van beállítva.)");
            Console.WriteLine("\nHasználató kulcsszavak: 'oct', '8' - oktális | 'hex', '16' - hexadecimális");
            
            bool isOct = false;

            Console.Write("A számrendszer alapja: ");
            string input = Console.ReadLine();

            if (input != "oct" && input != "8" && input != "hex" && input != "16")
            {
                do
                {
                    Console.Write("Hibás kulcsszó! Kérem adja meg újra a számrendszer alapját: ");
                    input = Console.ReadLine();
                } while (input != "oct" && input != "8" && input != "hex" && input != "16");
            }
            if (input == "oct" || input == "8")
            {
                isOct = true;
            }
            else if (input == "hex" || input == "16")
            {
                isOct = false;
            }         
            //-------------------------------Szám beírása---------------------------------------------------------------
            Console.Clear();
            Console.WriteLine("A program képes kezelni, hogy egy szám csak egészből, vagy törtrészből is áll!");
            Console.WriteLine("\nHasználható számjegyek: 0-tól 7-ig | Kérem, használjon tizedesvesszőt, ha törtszámot ad meg!\nMinta: 15,366 - [egész],[tört] ");
            if (isOct)
            {
                bool notOct = false;
                double octNum = 0;
                
                Console.Write("\nKérem írjon be egy oktális (8-as számrendszerbeli) számot: ");
                string inputNum = Console.ReadLine();
                //-------------------------------------Ellenőrzés---------------------------------------------------------
                do
                {
                    if (notOct)
                    {
                        Console.Write("Nem megfelelő bevitel! Kérem ügyeljen a vesszőkre, és a használható számjegyekre! Az új szám: ");
                        inputNum = Console.ReadLine();
                    }

                    notOct = false;

                    if (inputNum == "" || inputNum == "," || inputNum == ".") // A felhasználó nem írhat be csak vesszőt
                    {
                        notOct = true;
                        continue;
                    }

                    if (!double.TryParse(inputNum, out octNum)) // A felhasználó számot írt-e be
                    {
                        notOct = true;
                        continue;
                    }

                    if (double.Parse(inputNum) < 0)
                    {
                        notOct = true;
                        continue;
                    }

                    string[] inputTemp = inputNum.Split(','); // Tagonként megnézem, hogy megfelel-e az oktális számrendszernek-e a szám
                    for (int i = 0; i < inputTemp[0].Length; i++)
                    {
                        if (Convert.ToInt32(inputTemp[0][i].ToString()) > 7)
                        {
                            notOct = true;
                            break;
                        }
                    }

                    if (inputTemp.Length > 1)
                    {
                        for (int i = 0; i < inputTemp[1].Length; i++)
                        {
                            if (Convert.ToInt32(inputTemp[1][i].ToString()) > 7)
                            {
                                notOct = true;
                                break;
                            }
                        }
                    }
                } while (notOct);
                //--------------------------------Üres string kezelése--------------------------------------------------------------
                string[] octString = inputNum.Split(',');
                if (octString[0] == "")
                {
                    octString[0] = "0";
                }
                string octInt = "";
                string octFract = "";
                //--------------------------------Tárolás--------------------------------------------------------------
                if (octString.Length == 1)
                {
                    for (int i = 0; i < octString[0].Length; i++)
                    {
                        octInt += octString[0][i];
                    }
                }
                else
                {
                    for (int i = 0; i < octString[0].Length; i++)
                    {
                        octInt += octString[0][i];
                    }
                    octFract = octString[1];
                }
                //---------------------------------Decimálisra hozás-------------------------------------------------------------
                double octInDec = 0;
                double octPow = octInt.Length - 1;
                for (int i = 0; i < octInt.Length; i++)
                {
                    octInDec += Convert.ToDouble(Convert.ToString(octInt[i])) * Math.Pow(8, octPow);
                    octPow--;
                }
                //----------------------------------Törtrész 3-as csoportosítása------------------------------------------------------------
                string octFractBin = "";
                for (int i = 0; i < octFract.Length; i++)
                {
                    switch (octFract[i])
                    {
                        case '0':
                            octFractBin = octFractBin + "000";
                            break;
                        case '1':
                            octFractBin = octFractBin + "001";
                            break;
                        case '2':
                            octFractBin = octFractBin + "010";
                            break;
                        case '3':
                            octFractBin = octFractBin + "011";
                            break;
                        case '4':
                            octFractBin = octFractBin + "100";
                            break;
                        case '5':
                            octFractBin = octFractBin + "101";
                            break;
                        case '6':
                            octFractBin = octFractBin + "110";
                            break;
                        case '7':
                            octFractBin = octFractBin + "111";
                            break;
                        default:
                            Console.WriteLine("Hiba!"); // NEM SZABAD IDE LÉPNED!!!
                            break;
                    }
                }
                //------------------------------Törtrész "felkészítése" 4-es csoportosításra----------------------------------------------------------------
                if (octFractBin.Length % 4 == 1)
                {
                    octFractBin = octFractBin + "000";
                }
                else if (octFractBin.Length % 4 == 2)
                {
                    octFractBin = octFractBin + "00";
                }
                else if (octFractBin.Length % 4 == 3)
                {
                    octFractBin = octFractBin + "0";
                }
                //--------------------------------------Törtrész 4-es csoportosítása--------------------------------------------------------
                int fractIndex = 0;
                string hexFract = "";
                string octTemp = "";

                while (fractIndex < octFractBin.Length)
                {
                    octTemp = octFractBin[fractIndex].ToString() + octFractBin[fractIndex + 1].ToString() + octFractBin[fractIndex + 2].ToString() + octFractBin[fractIndex + 3].ToString();
                    switch (octTemp)
                    {
                        case "0000":
                            hexFract = hexFract + "0";
                            break;
                        case "0001":
                            hexFract = hexFract + "1";
                            break;
                        case "0010":
                            hexFract = hexFract + "2";
                            break;
                        case "0011":
                            hexFract = hexFract + "3";
                            break;
                        case "0100":
                            hexFract = hexFract + "4";
                            break;
                        case "0101":
                            hexFract = hexFract + "5";
                            break;
                        case "0110":
                            hexFract = hexFract + "6";
                            break;
                        case "0111":
                            hexFract = hexFract + "7";
                            break;
                        case "1000":
                            hexFract = hexFract + "8";
                            break;
                        case "1001":
                            hexFract = hexFract + "9";
                            break;
                        case "1010":
                            hexFract = hexFract + "A";
                            break;
                        case "1011":
                            hexFract = hexFract + "B";
                            break;
                        case "1100":
                            hexFract = hexFract + "C";
                            break;
                        case "1101":
                            hexFract = hexFract + "D";
                            break;
                        case "1110":
                            hexFract = hexFract + "E";
                            break;
                        case "1111":
                            hexFract = hexFract + "F";
                            break;
                        default:
                            Console.WriteLine("Hiba!"); // Ha idelépsz, akkor baj van!
                            break;
                    }
                    fractIndex = fractIndex + 4;
                }
                //--------------------------------Kiírás és befejezés--------------------------------------------------------------
                string octInHex = Convert.ToString((int)octInDec, 16);
                bool beauty = false; // Akkor igaz, ha "szépíteni" kell a számot
                if (octInt == "")
                {
                    octInt = "0";
                }
                if (octFract == "")
                {
                    octFract = "0";
                    beauty = true;
                }
                if (hexFract == "" && beauty)
                {
                    Console.WriteLine("\nA {0} oktális szám átváltva hexadecimálisba: {1}", octInt, octInHex.ToUpper());
                }
                else if (hexFract == "")
                {
                    Console.WriteLine("\nA {0},{1} oktális szám átváltva hexadecimálisba: {2}", octInt, octFract, octInHex.ToUpper());
                }
                else
                {
                    Console.WriteLine("\nA {0},{1} oktális szám átváltva hexadecimálisba: {2},{3}", octInt, octFract, octInHex.ToUpper(), hexFract);
                }
            }
            else //------------------------------16-os számrendszeri ág--------------------------------------------------------------------------------
            {
                Console.Clear();
                Console.WriteLine("A program képes kezelni, hogy egy szám csak egészből, vagy törtrészből is áll!");
                bool notHex = false;
                bool hexIntBool = false;
                Console.WriteLine("\nHasználható számjegyek: 0-tól 9-ig, valamint A = 10, B = 11, ... F = 15 | Kérem, használjon tizedesvesszőt, ha törtszámot ad meg!\nMinta: 6F,CA1 - [egész],[tört] ");
                //-------------------------------Szám beírása---------------------------------------------------------------
                Console.Write("\nKérem írjon be egy hexadecimális (16-as számrendszerbeli) számot: ");
                string inputNum = Console.ReadLine();
                string hexInput = "";
                //-------------------------------Ellenőrzés---------------------------------------------------------------
                do
                {
                    if (notHex)
                    {
                        Console.Write("Nem megfelelő bevitel! Kérem ügyeljen a vesszőkre, és a használható számjegyekre, illetve betűkre! Az új szám: ");
                        inputNum = Console.ReadLine();
                    }
                    
                    notHex = false;

                    if (inputNum == "" || inputNum == "," || inputNum == ".") // A felhasználó nem írhat be csak vesszőt
                    {
                        notHex = true;
                        continue;
                    }
                    
                    string[] inputTemp = inputNum.Split(','); // Tagonként megnézem, hogy megfelel-e a hexadecimális számrendszernek-e a szám
                    if (inputTemp.Length > 2)
                    {
                        notHex = true;
                        continue;
                    }
                    
                    hexIntBool = false;
                    if (inputTemp.Length == 1) // A szám csak egész (I/H)
                    {
                        hexIntBool = true;
                    }
                    
                    hexInput = "";
                    for (int i = 1; i < inputTemp[0].Length; i++)
                    {
                        // 16-os számrendszernek megfelel-e :)
                        if ((inputTemp[0][i].ToString() == "0" || inputTemp[0][i].ToString() == "1" || inputTemp[0][i].ToString() == "2" || inputTemp[0][i].ToString() == "3" || inputTemp[0][i].ToString() == "4" || inputTemp[0][i].ToString() == "5" || inputTemp[0][i].ToString() == "6" || inputTemp[0][i].ToString() == "7" || inputTemp[0][i].ToString() == "8" || inputTemp[0][i].ToString() == "9" || inputTemp[0][i].ToString() == "a" || inputTemp[0][i].ToString() == "A" || inputTemp[0][i].ToString() == "b" || inputTemp[0][i].ToString() == "B" || inputTemp[0][i].ToString() == "c" || inputTemp[0][i].ToString() == "C" || inputTemp[0][i].ToString() == "d" || inputTemp[0][i].ToString() == "D" || inputTemp[0][i].ToString() == "e" || inputTemp[0][i].ToString() == "E" || inputTemp[0][i].ToString() == "f" || inputTemp[0][i].ToString() == "F"))
                        {
                            hexInput = hexInput + inputTemp[0][i].ToString().ToUpper();
                        }
                        else
                        {
                            notHex = true;
                            break;
                        }
                    }
                    //-----------------------------------------Törtrész lekezelése------------------------------------------------------------------
                    if (!hexIntBool)
                    {
                        hexInput = hexInput + ",";
                        for (int i = 0; i < inputTemp[1].Length; i++)
                        {
                            // Rémálom, DE MŰKÖDIK!!!
                            if ((inputTemp[1][i].ToString() == "0" || inputTemp[1][i].ToString() == "1" || inputTemp[1][i].ToString() == "2" || inputTemp[1][i].ToString() == "3" || inputTemp[1][i].ToString() == "4" || inputTemp[1][i].ToString() == "5" || inputTemp[1][i].ToString() == "6" || inputTemp[1][i].ToString() == "7" || inputTemp[1][i].ToString() == "8" || inputTemp[1][i].ToString() == "9" || inputTemp[1][i].ToString() == "a" || inputTemp[1][i].ToString() == "A" || inputTemp[1][i].ToString() == "b" || inputTemp[1][i].ToString() == "B" || inputTemp[1][i].ToString() == "c" || inputTemp[1][i].ToString() == "C" || inputTemp[1][i].ToString() == "d" || inputTemp[1][i].ToString() == "D" || inputTemp[1][i].ToString() == "e" || inputTemp[1][i].ToString() == "E" || inputTemp[1][i].ToString() == "f" || inputTemp[1][i].ToString() == "F"))
                            {
                                hexInput = hexInput + inputTemp[1][i].ToString().ToUpper();
                            }
                            else
                            {
                                notHex = true;
                                break;
                            }
                        }
                    }
                    
                } while (notHex);
                //-----------------------------------------------Negatív kezelés----------------------------------------------------------------
                bool hexNeg = false; // Bent kellett hagynom, hogy le tudjam kezelni a későbbiekben a kiírásnál a számokat (nincs már haszna)
                string[] hexString = inputNum.Split(',');
                string hexInt = "";
                string hexFract = "";

                if (hexString[0] == "")
                {
                    hexString[0] = "0";
                }

                if (hexString[0][0] == '-')
                {
                    hexNeg = true;
                }
                //----------------------------------------Negatív számnál a '-' kihagyása-------------------------------------------------------------
                if (hexNeg)
                {
                    for (int i = 1; i < hexString[0].Length; i++)
                    {
                        hexInt += hexString[0][i].ToString().ToUpper();
                    }
                    if (hexString.Length > 1)
                    {
                        hexFract = hexString[1].ToUpper();
                    }
                }
                else
                {
                    if (hexString.Length == 1)
                    {
                        hexInt = hexString[0].ToUpper();
                    }
                    else
                    {
                        hexInt = hexString[0].ToUpper();
                        hexFract = hexString[1].ToUpper();
                    }
                }
                //----------------------------------Egészrész átalakítása-------------------------------------------------------------
                double hexInDec = 0;
                double hexPow = hexInt.Length - 1;
                for (int i = 0; i < hexInt.Length; i++)
                {
                    switch (hexInt[i])
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            hexInDec += Convert.ToDouble(Convert.ToString(hexInt[i])) * Math.Pow(16, hexPow);
                            break;
                        case 'A':
                            hexInDec += Convert.ToDouble(10 * Math.Pow(16, hexPow));
                            break;
                        case 'B':
                            hexInDec += Convert.ToDouble(11 * Math.Pow(16, hexPow));
                            break;
                        case 'C':
                            hexInDec += Convert.ToDouble(12 * Math.Pow(16, hexPow));
                            break;
                        case 'D':
                            hexInDec += Convert.ToDouble(13 * Math.Pow(16, hexPow));
                            break;
                        case 'E':
                            hexInDec += Convert.ToDouble(14 * Math.Pow(16, hexPow));
                            break;
                        case 'F':
                            hexInDec += Convert.ToDouble(15 * Math.Pow(16, hexPow));
                            break;
                        default:
                            Console.WriteLine("Hiba!"); // :)
                            break;
                    }
                    hexPow--;
                }
                //--------------------------------Törtrész binárissá alakítása------------------------------------------------------------------
                string hexFractBin = "";
                for (int i = 0; i < hexFract.Length; i++)
                {
                    switch (hexFract[i])
                    {
                        case '0':
                            hexFractBin = hexFractBin + "0000";
                            break;
                        case '1':
                            hexFractBin = hexFractBin + "0001";
                            break;
                        case '2':
                            hexFractBin = hexFractBin + "0010";
                            break;
                        case '3':
                            hexFractBin = hexFractBin + "0011";
                            break;
                        case '4':
                            hexFractBin = hexFractBin + "0100";
                            break;
                        case '5':
                            hexFractBin = hexFractBin + "0101";
                            break;
                        case '6':
                            hexFractBin = hexFractBin + "0110";
                            break;
                        case '7':
                            hexFractBin = hexFractBin + "0111";
                            break;
                        case '8':
                            hexFractBin = hexFractBin + "1000";
                            break;
                        case '9':
                            hexFractBin = hexFractBin + "1001";
                            break;
                        case 'A':
                            hexFractBin = hexFractBin + "1010";
                            break;
                        case 'B':
                            hexFractBin = hexFractBin + "1011";
                            break;
                        case 'C':
                            hexFractBin = hexFractBin + "1100";
                            break;
                        case 'D':
                            hexFractBin = hexFractBin + "1101";
                            break;
                        case 'E':
                            hexFractBin = hexFractBin + "1110";
                            break;
                        case 'F':
                            hexFractBin = hexFractBin + "1111";
                            break;
                        default:
                            Console.WriteLine("Hiba!"); // Ha idelépsz, akkor kaka vagy! :)
                            break;
                    }
                }
                //--------------------------------------3-as csoportosításra "előkészítés"--------------------------------------------------------------------------
                if (hexFractBin.Length % 3 == 1)
                {
                    hexFractBin = hexFractBin + "00";
                }
                else if (hexFractBin.Length % 3 == 2)
                {
                    hexFractBin = hexFractBin + "0";
                }
                //------------------------------------3-as csoportosítás----------------------------------------------------------------------------
                int fractIndex = 0;
                string octFract = "";
                string hexTemp = "";

                while (fractIndex < hexFractBin.Length)
                {
                    hexTemp = hexFractBin[fractIndex].ToString() + hexFractBin[fractIndex + 1].ToString() + hexFractBin[fractIndex + 2].ToString();
                    switch (hexTemp)
                    {
                        case "000":
                            octFract = octFract + "0";
                            break;
                        case "001":
                            octFract = octFract + "1";
                            break;
                        case "010":
                            octFract = octFract + "2";
                            break;
                        case "011":
                            octFract = octFract + "3";
                            break;
                        case "100":
                            octFract = octFract + "4";
                            break;
                        case "101":
                            octFract = octFract + "5";
                            break;
                        case "110":
                            octFract = octFract + "6";
                            break;
                        case "111":
                            octFract = octFract + "7";
                            break;
                        default:
                            Console.WriteLine("Hiba!"); // Ha idelépsz, akkor kaka vagy! :)
                            break;
                    }
                    fractIndex = fractIndex + 3;
                }
                //------------------------------------------Kiírás és befejezés-------------------------------------------------------------------------------------
                string hexInOct = Convert.ToString((int)hexInDec, 8);
                bool beauty = false; // Akkor igaz, ha "szépíteni" kell a számot
                if (hexInt == "")
                {
                    hexInt = "0";
                }
                if (hexFract == "")
                {
                    hexFract = "0";
                    beauty = true;
                }
                if (hexNeg)
                {
                    if (octFract == "" && beauty)
                    {
                        Console.WriteLine("\nA {0} hexadecimális szám átváltva oktálisba: {1}", hexInt, hexInOct);
                    }
                    else if (octFract == "")
                    {
                        Console.WriteLine("\nA {0},{1} hexadecimális szám átváltva oktálisba: {2}", hexInt, hexFract, hexInOct);
                    }
                    else
                    {
                        Console.WriteLine("\nA {0},{1} hexadecimális szám átváltva oktálisba: {2},{3}", hexInt, hexFract, hexInOct, octFract);
                    }
                }
                else
                {
                    if (octFract == "" && beauty)
                    {
                        Console.WriteLine("\nA {0} hexadecimális szám átváltva oktálisba: {1}", hexInt, hexInOct);
                    }
                    else if (octFract == "")
                    {
                        Console.WriteLine("\nA {0},{1} hexadecimális szám átváltva oktálisba: {2}", hexInt, hexFract, hexInOct);
                    }
                    else
                    {
                        Console.WriteLine("\nA {0},{1} hexadecimális szám átváltva oktálisba: {2},{3}", hexInt, hexFract, hexInOct, octFract);
                    }
                }
            }
            //----------------------------------------Leállítás-----------------------------------------------------------------------------
            Console.WriteLine("\nA program egy gomb lenyomására leáll!");
            Console.ReadKey();
        }
    }
}