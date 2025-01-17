This is a POC project.

Project MVP:
End goal of this project is to run Linux on a RaspberryPi, which is using SocketCAN to constantly stream vehicle data from a PCAN-USB and log data onto an SD Card.
It will run a web server that can be connected over local wifi, and will include a basic web interface.

When triggered through the web interface, the app will load all logged data into a KISS formatted file, and drop into a folder.
Direwolf and KISSUtil will be running and monitoring the folder.  When the file is dropped, the data will be read and sent over HAM radio as AX.25 packet data.
Breaking into multiple smaller files would introduce overhead, but improve retry-ability. 

A laptop will be hooked to a Ham radio elsewhere, which is constantly listening for this incoming data.  The laptop will have its own separate app that will convert the data into a CSV file for later consumption.


Beyond MVP additional hardening can be built that could include 2-way communication for acknowledging receipt, and requesting re-send of specific packets.   We could also build a sort of pre-handshake prior to sending data.
