import type { User } from "./user";

export interface Message {
  id: number;
  content: string;
  createAt?: string;
  conversationId: number;
  senderId: string;
  sender?: User;
}

export interface MessageCreate {
  content: string;
  conversationId: number;
  senderId: string;
}

export enum ConversationStatus {
  Active = 1,
  Closed = 2,
  Pending = 3,
  Archived = 4,
}

export interface Conversation {
  id: number;
  name?: string;
  status: ConversationStatus;
  customerId: string;
  employeeId?: string;
  customer?: User;
  employee?: User;
  messages?: Message[];
}
