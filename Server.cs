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
 * to tell knock-knock jokes repeatedly. The server is a console application that prints out the client's replies as well as any issues 
 * that occur through TCP/IP. Moreover, If the server receives an incorrect response, this program indicate that what the client should 
 * have stated and then waits for the correct response from the client.
 * 
 * File: Server.cs
 * 
 * Purpose: For assignment 1, COIS 3040, I wrote server code to convey jokes to clients via sockets.
 *
 * Uasage: This server code must run before the client code.
 * 
 * Description of parameters: none
 * 
 * Namespace required: SynchronousSocketServer
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace SynchronousSocketServer
{
    public class Server
    {
        //incoming data from the client
        private static string messageRec_1 = string.Empty;
        private static string messageRec_2 = string.Empty;


        // creating the private static method of Random class
        private static Random randomArray = new Random();

        //Creating the private static method array string of knock knock names to send these randomly to the client.
        private static string[] knockName = { "Says",
                                     "Water",
                                     "Nobel",
                                     "Annie",
                                     "Wa",
                                     "I am",
                                     "Haven",
                                     "Spell",
                                     "To",
                                     "Tank"
        };

        //Creating the proivate static method array string of knock knock jokes that will be sent to the client on the basis of the input from the user.
        private static string[] knockJoke = {
                                    "Says me, that's who!",
                                    "Water you asking so many questions for, just open up!",
                                    "Nobel ... that's why I knocked!",
                                    "Annie thing you can do I can better!",
                                    "What are you so excited about?!><",
                                    "Dont you even know who you are? :/",
                                    "Haven you heard enough of these knock-knock jokes?",
                                    "W-H-O!",
                                    "No, it's to whom!",
                                    "You are welcome"
        };

        //Main function! Entry point
        public static int Main(string[] args)
        {
            
            //start the server -> it will connect the server to client and waits for the client to get connected.
            Listening_fromClient();
            Console.ReadLine();
            return 0;

        }//end Main


        //Purpose: creates a socket and listens for incoming connections. Also, connect to the client application.
        public static void Listening_fromClient()
        {


            //a flag for our main loop -> this loop will run untill it's true. 
            bool mainLoopRunning = true;

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the host running the application.  
            IPHostEntry ipHost_Info = Dns.GetHostEntry(Dns.GetHostName());
            //from the list of addresses, get the first ip address
            IPAddress IP_Address = ipHost_Info.AddressList[0];

            //An IP address and a port are encapsulated in the IPEndPoint class. Reference: https://docs.microsoft.com/en-us/dotnet/api/system.net.ipendpoint?view=net-6.0
            IPEndPoint localEndPoint = new IPEndPoint(IP_Address, 11000);

            // To send and receive data, creates a TCP/IP socket.
            Socket listener = new Socket(IP_Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);



            try
            {


                //sets the listener Socket to the selected IP and port	
                listener.Bind(localEndPoint);

                //Listen simply instructs the socket to keep an eye out for new connections. The argument 10 specifies the length of the incoming queue.
                listener.Listen(10);

                // Begin to listen for connections.
                // The mainLoopRunning is a boolean value that will run untill the client connects to the socket.
                // This loop gives benifit to the client, if the client terminal closed and when the client again run the terminal it will be automatically connected to it.
                while (mainLoopRunning)
                {

                    //Printing this message on the server terminal
                    Console.WriteLine("\nWaiting for a connection...");


                    Socket handler = listener.Accept();

                    

                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("Connection Established!");
                    Console.WriteLine("-----------------------------");

                    //Method: interactWithClient(handler)
                    //params: handler, which is a socket. 
                    /*Purpose:  This function provides the code for printing client responses as well as any failure conditions encountered via TCP/IP.
                                This function will provide cues for the delivery of the joke.
                                If the server receives an incorrect response, it responds by sending a message to the client explaining
                                what should have been stated and waiting for the corrected response.*/

                    interactWithClient(handler);

                    // this loop will run even after the client quit it's application. So that, it allows the client to easily connect with the server just by 
                    // running the client executable file.
                    mainLoopRunning = true;


                }

            }//end try block
            catch (TimeoutException te)
            {
                Console.WriteLine(te.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());        //prints the error encountered while running the code in try block.
            }  //end catch block



        }//end StartListening


        public static void interactWithClient(Socket handler)
        {
            try
            {

                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];

                //loop to get all our valid input
                //This for(;;) loop will continously run untill the user enter 'quit' in there messages.
                for (; ; )
                {
                    //ValidOuterInput is a boolean value that will tell the outer while loop that desired input is received or not.
                    bool ValidOuterInput = false;
                    //ValidInputLoop is a boolean value that indicates whether or not the required input has been received in the inner while loop.
                    bool ValidInnerLoop = false;

                    // Send the 'Knock Knock' message to the client in bytes.
                    byte[] knockknockMessage = Encoding.ASCII.GetBytes("Knock Knock!");
                    //send our reply
                    handler.Send(knockknockMessage); //Message sent 'Knock Knock!'

                    //This is the outer while loop that will run untill Valid input message recieved from the client.
                    //ValidOuterInput get's only true when the message recieved gets equal to the desired message. When it get's true, loop breaks.
                    while (!ValidOuterInput)
                    {
                        //This string get empty whenever the while loop repeats. So, that the string don't hold the previous value. Otherwise, it will add strings again and again.
                        messageRec_1 = string.Empty;

                        //receiving some bytes (message from the client)
                        int bytesRec_1 = handler.Receive(bytes);


                        // Converting the bytes server received into a String
                        // The param1 is the bytes, param2 is an offset, and param3 is the length of recieved message
                        messageRec_1 += Encoding.ASCII.GetString(bytes, 0, bytesRec_1);

                        //if the recieved message from the client get's equal to 'Who's there? or 'Banana who?' This if block will run.
                        //It contains the code to recieve and send message to the Client through a TCP/IP in order to send knock knock jokes.
                        if (messageRec_1 == "Who's there?")
                        {
                            //outer while loop will be breaked. As the server got the desired input :).
                            ValidOuterInput = true;

                            //The System's Next() method. To generate random strings from an array = knockName, the Random is utilised.
                            int rand = randomArray.Next(knockName.Length);

                            // Show the data received on the server console.  
                            Console.WriteLine("Text received : {0}", messageRec_1);

                            //sending a string randomly from Array: knockName. The message is sent in bytes.
                            byte[] bytesSent_1 = Encoding.ASCII.GetBytes(knockName[rand]);

                            handler.Send(bytesSent_1); //send knock name randomly from an array 

                            //It is the inner while loop that works in a same way as the outer while loop
                            //ValidInputLoop get's only true when the message sent to the server gets equal to the valid message. When it get's true, loop breaks.
                            while (!ValidInnerLoop)
                            {

                                messageRec_2 = string.Empty;         //Stroring a second time message recieving from the client in an empty string

                                int bytesRec_2 = handler.Receive(bytes);    //actually receive some bytes

                                //Converting a byte message recieved into a String
                                messageRec_2 += Encoding.ASCII.GetString(bytes, 0, bytesRec_2);

                                //if the recieved message from the client get's equal to the message sent previously from the server + a string of who? with it. This if block will run.
                                //It contains the code to recieve and send message to the client through TCP/IP in order to get knock knock jokes.
                                if (messageRec_2 == knockName[rand] + " who?")
                                {
                                    //inner while loop will be breaked. As the server got the desired input :).
                                    ValidInnerLoop = true;

                                    // Show the data on the console.  
                                    Console.WriteLine("Text received : {0}", messageRec_2);

                                    //Sending another message to a client randomly from a array: knockJoke.
                                    byte[] bytesSent_2 = Encoding.ASCII.GetBytes(knockJoke[rand]);

                                    handler.Send(bytesSent_2);  //send our reply
                                }
                                //If the message recieved to the server gets equal to 'quit'. It will call the ServerDisconnect() function.
                                else if (messageRec_2 == "quit")
                                {
                                    //param1 is the byte, param2 is the Socket, para3 is the message recieved from the client
                                    ServerDisconnect(bytes, handler, messageRec_2);
                                    return;

                                }
                                else
                                {
                                    ValidInnerLoop = false;
                                    //Printing on the server as we recieved a Invalid Text
                                    Console.WriteLine("Invalid Text received : {0}", messageRec_2);
                                    //Invalid message error is sent to the Client in bytes to input the valid text.
                                    byte[] messageInvalid_2 = Encoding.ASCII.GetBytes("Error:Please input a valid string -> recieved_Name + who?. \n\tFor Example: 'Sid who?'. Ask Now properly! I am knocking");
                                    handler.Send(messageInvalid_2); //sending our Message 

                                }
                            }//end of inner while loop
                        }
                        //If the message recieved to the server gets equal to 'quit'. It will call the ServerDisconnect() function.
                        else if (messageRec_1 == "quit")
                        {
                            //param1 is the byte, param2 is the Socket, para3 is the message recieved from the client
                            ServerDisconnect(bytes, handler, messageRec_1);
                            return;

                        }
                        else
                        {
                            Console.WriteLine("Invalid Text received : {0}", messageRec_1);
                            //Invalid message error is sent to the Client in bytes to input the valid text.
                            byte[] messageInvalid_1 = Encoding.ASCII.GetBytes("Please enter a valid response -> 'Who's there?'");
                            //sending our reply
                            handler.Send(messageInvalid_1);
                        }


                    }//end of outer while loop

                }//end for loop
            }
            catch (Exception ex)
            {
                //print any error it encounters while running the code inside the try block
                Console.WriteLine(ex.ToString());
            }
        }

        //This function will only called or worked when the client enter the 'quit' message in the terminal application.
        ////param1 is the byte, param2 is the Socket, para3 is the message recieved from the client
        public static void ServerDisconnect(byte[] bytes, Socket handler, string dataExit)
        {

            //receiving some bytes from the client
            int bytesRec_Exit = handler.Receive(bytes);
            //Converting the bytes received into a String.
            dataExit += Encoding.ASCII.GetString(bytes, 0, bytesRec_Exit);
            Console.WriteLine("Text received : {0}", dataExit);
            //clean up and shut down the new socket created for this connection
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            return; //return because the client quit it's application. This server will still wait for the connection.
        }

    }//End Class Server
}// end namespace