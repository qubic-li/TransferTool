# TransferTool
The qubic.li TransferTool is an early stage tool to test transfers/transaction in the qubic network. You may use this tool to propagate transactions into the qubic network via the qubic.li API.

We do not guarentee a successfull delivery of transactions to the network. The use of this tool is without any warranty.

Usage:

```
transfertool.exe <SOURCESEED> <DESTINATIONID> <AMOUNT> [TICK]
```

<SOURCESEED> Your Seed from which you would like to send Energy/quos (55 lower case letters)
<DESTINATIONID> The Destination ID to which you want to send the Energy (60 upper case letters)
<AMOUNT> The Amount of Energy/quos to send
[TICK] OPTIONAL The tick to execute this transaction. If left empty it take current Tick + 10

The tool is right now not doing any check if the entered values are correct.
