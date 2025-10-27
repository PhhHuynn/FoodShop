import * as signalR from "@microsoft/signalr";

const hubConnection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7108/chathub")
  .withAutomaticReconnect()
  .build();

export default hubConnection;
