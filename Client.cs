/*
 * Name: Sidak Singh Sra
 * Student ID: 0689168
 * 
 * COIS3040 Assignment #1
 * 
 * Description: With the help of some provided code, I have implemented an application that will communicate through TCP/IP.   
 * Used this code: To connect the Server with client using sockets in c# -> Reference: Microsoft sample code: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-server-socket-example.
 * 
 * Based on the client-server architectural model, I have a created and implemented an application that will communicate through TCP/IP 
 * to tell knock-knock jokes repeatedly between server and client. The client is a terminal program that allows the user to respond to prompts by typing. 
 * When the user input something in the terminal that message is delivered to the server through TCP/IP. Moreover, If the client send
 * an incorrect response, the server indicates that what the client should have stated and then the server wait for the peoper response input by the client.
 * 
 * File: Client.cs
 * 
 * Purpose: For Assignment 1, COIS 3040, I wrote this client code to send input message and receive jokes from the server via sockets.
 *
 * Uasage: The server code must run before this client code.
 * 
 * Namespace required: SynchronousSocketClient
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SynchronousSocketClient
{
    /*Class: Client
    Purpose: The client code is contained in this class.*/
    public class Client
    {
        //Incoming data from the server
        private static string knockMessage = string.Empty;
        private static string ServerMessage_1 = string.Empty;
        private static string ServerMessage_err = string.Empty;
        private static string ServerMessage_2 = string.Empty;
        private static string ServerMessage_err1 = string.Empty;

        //Sending data to the server
        private static string messageSent_1 = string.Empty;
        private static string messageSent_2 = string.Empty; 
        

        //Main Function -> Entry point to start the program
        public static int Main(string[] args)
        {
            /*Method: ConnectWithServer
            Purpose: ConectWithServer contain the code to connect the client application to the server and a another method: interactWithServer() .*/
            ConnectwithServer();
            Console.ReadLine();
            return 0;
        }

        /*Purpose: creates a socket and Connect with the server.*/
        public static void ConnectwithServer()
        {

            // try block contain the code to connect the client to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This uses port 11000 on the local computer.
                IPHostEntry ipHost_Info = Dns.GetHostEntry(Dns.GetHostName());

                //take the first IP address in the list.
                IPAddress IP_Address = ipHost_Info.AddressList[0];

                //Make an object out of our IP address and port.
                IPEndPoint remoteEndPoint = new IPEndPoint(IP_Address, 11000);

                //This creates a TCP/IP socket.
                Socket sender = new Socket(IP_Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Try block contains the code to connect the socket to the remote endpoint.  
                try
                {
                    //calling the server
                    sender.Connect(remoteEndPoint);

                    //Indicate the client that ocket is connected to this port.
                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    /*Method: interactWithServer(sender)
                    Purpose: To receive and send messages to the server across a TCP/IP socket using the data entered by the client.
                    params: sender - > (it's a socket)
                    */

                    interactWithServer(sender);

                }//end of inner data-sending try

                //catch any errors, if try fails
                catch (ArgumentNullException ane)
                {
                    //print any error that it encounters on the client application
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    //Print any error if the socket connection fails
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    //any other unexpected error
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }


            }//end outer try for connection

            //Catch any error, if outer try block fails.
            catch (Exception e)
            {
                //print the error message on the client application
                Console.WriteLine(e.ToString());
            }

        }//end StartClient

        /*Method: interactWithServer(Socket sender)
         Purpose: interWithServer(Socket sender) contains the code that allows the user to write in messages that will be transmitted 
         to the server through TCP/IP, and the server will then interact with the client in such a way that the server responds to the 
         client's messages and tells jokes, as well as any errors that may occur and wait for the client to enter again the proper response. */
        public static void interactWithServer(Socket sender)
        {
            try
            {
                // Data buffer for incoming data (from the server).  
                byte[] bytes = new byte[1024];

                //This for(;;) loop will continously run untill the user enter 'quit' in there messages.
                for (; ; )
                {
                    //knockMessage is a empty string. That will be filled when the server sends the 'Knock Knock' message!
                    knockMessage = string.Empty;

                    //ValidOuter_Input is a boolean value that will tell the outer while loop that desired input is received or not.
                    bool ValidOuter_Input = false;
                    //ValidInputLoop is a boolean value that indicates whether or not the required input has been received in the inner while loop.
                    bool ValidInputLoop = false;

                    // Receive the 'Knock Knock' message from the server in bytes.  
                    int knockReceive = sender.Receive(bytes);

                    //the knock knock messaage is converted from bytes into a data string to print message on the client application.
                    knockMessage += Encoding.ASCII.GetString(bytes, 0, knockReceive);

                    Console.WriteLine("\nServer: " + knockMessage); //printing the knock knock message on the client terminal.

                    //This is the outer while loop that will run untill Valid input message recieved from the client.
                    //ValidOuter_Input get's only true when the message sent gets equal to the desired message. When it get's true, loop breaks.
                    while (!ValidOuter_Input)
                    {
                        //These strings get empty whenever the while loop repeats. So, that the string don't hold the previous value. Otherwise, it will add strings again and again.
                        ServerMessage_1 = string.Empty; ServerMessage_err = string.Empty; ServerMessage_2 = string.Empty; messageSent_1 = string.Empty;
                        //prompt the user to enter the message in order to send it to the server.
                        Console.Write("Client: ");
                        messageSent_1 = Console.ReadLine(); //Take the input from the user.

                        //if the input message from the client get's equal to 'Who's there? or 'Banana who?' This if block will run.
                        //It contains the code to recieve and send message to the server through TCP/IP in order to get knock knock jokes.
                        if (messageSent_1 == "Who's there?")
                        {
                            //outer while loop will be breaked. As the client enter the desired input :).
                            ValidOuter_Input = true;

                            //Sending Message to the server that the client entered in messageSent_1 in form of a byte.
                            byte[] SendMessage_1 = Encoding.ASCII.GetBytes(messageSent_1);
                            sender.Send(SendMessage_1); //message sent

                            //Receiving the message from the server based on the keyword that the client entered.
                            int bytesReceive_1 = sender.Receive(bytes);

                            //Converting message recieved from byte to a data string.
                            ServerMessage_1 += Encoding.ASCII.GetString(bytes, 0, bytesReceive_1);

                            //Printing the Server message that the client recieved on the basis of the input string.
                            //Basically, here client is receiving the knock knock joke name from the server.
                            Console.WriteLine("Server: " + ServerMessage_1);

                            //It is the inner while loop that works in a same way as the outer while loop
                            //ValidInputLoop get's only true when the message sent to the server gets equal to the valid message. When it get's true, loop breaks.
                            while (!ValidInputLoop)
                            {
                                //These empty strings get's empty again whenever the while loop repeats.So, that the string don;t hold the previous value.
                                messageSent_2 = string.Empty; ServerMessage_err1 = string.Empty;
                                //prompt the user to enter the message in order to send it to the server and recieve a valid joke based on the input.
                                Console.Write("Client: ");
                                messageSent_2 = Console.ReadLine(); //taking input from the client

                                //if the input message from the client get's equal to the message recieved previously + a string of who? with it. This if block will run.
                                //ServerMessage_1 is the message which the client recieved previously from the server. For example-> if client recieved 'Sid'. then in this client will enter 'Sid who?'
                                //It contains the code to recieve and send message to the server through TCP/IP in order to get knock knock jokes.
                                if (messageSent_2 == ServerMessage_1 + " who?")
                                {
                                    //inner while loop will be breaked. As the client entered the desired input :).
                                    ValidInputLoop = true;

                                    //Sending Message to the server that the client entered in messageSent_2 in form of a byte.
                                    byte[] SendMessage_2 = Encoding.ASCII.GetBytes(messageSent_2);
                                    sender.Send(SendMessage_2); //Message sent

                                    //Receiving the message from the server based on the valid keyword that the client entered.
                                    int bytesReceive_2 = sender.Receive(bytes);

                                    //Converting message recieved from byte to a data string.
                                    ServerMessage_2 += Encoding.ASCII.GetString(bytes, 0, bytesReceive_2);

                                    //Printing the Server message that the client recieved on the basis of the input string.
                                    //Here client is receiving the knock knock jokes from the server.
                                    Console.WriteLine("Server: " + ServerMessage_2);
                                }//end of inner if statement

                                //If the message sent to the server gets equal to 'quit'. It will call the ClientExit() function.
                                else if (messageSent_2 == "quit")
                                {
                                    //params: sender and messageSent_2
                                    ClientExit(sender, messageSent_2);
                                }
                                else
                                {

                                    ValidInputLoop = false;                                   // the loop will again run as the client got an error
                                    byte[] msg_err1 = Encoding.ASCII.GetBytes(messageSent_2); //Message is being sent to the server in form of bytes
                                    sender.Send(msg_err1);                                    //message sent
                                    int bytesRec_err1 = sender.Receive(bytes);                // Receiving the error message from the server
                                    ServerMessage_err1 += Encoding.ASCII.GetString(bytes, 0, bytesRec_err1);    //Converting bytes to data string.
                                    Console.WriteLine("Server: " + ServerMessage_err1);       //printing the message received from the server
                                }
                            }
                        }// end of outer if statement

                        //If the message sent from the client to the server gets equal to 'quit'. It will call the ClientExit() function.
                        //params: sender and messageSent_1
                        else if (messageSent_1 == "quit")
                        {

                            ClientExit(sender, messageSent_1);

                        }

                        //If the client does not enter the valid input, it will send the message to the server in form of TCP/IP
                        //and then the server will send the error message about what the client should have said or what is the valid input.
                        else
                        {
                            byte[] msg_err = Encoding.ASCII.GetBytes(messageSent_1);    //Message is being sent to the server in form of bytes
                            sender.Send(msg_err);
                            int bytesRec_err = sender.Receive(bytes);                   // Receiving the error message from the server
                            ServerMessage_err += Encoding.ASCII.GetString(bytes, 0, bytesRec_err);
                            Console.WriteLine("Server: " + ServerMessage_err);          //printing the message received from the server
                        }

                    }


                }
            }//end of try block
            //Catch any error if it encounters during the execution of the try block
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        //This function will only called or worked when the client enter the 'quit' message in the terminal application.
        //Params: sender, SentMssage_quit
        public static void ClientExit(Socket sender, string SentMessage_quit)
        {
            //'quit' Message is send to the server in form of bytes and this message will be printed on ther server application.
            byte[] bytesSent_quit = Encoding.ASCII.GetBytes(SentMessage_quit);
            sender.Send(bytesSent_quit);
            // Release the socket.  
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            //Close the client terminal application when this function works
            Environment.Exit(-1);
        }


    }//end class Client

}//end namespace