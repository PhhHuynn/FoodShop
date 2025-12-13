export interface Message {
  id: number;
  content: string;
  createAt?: string;
  conversationId: number;
  senderId: string;
  senderName: string;
  senderType: string;
}

export interface MessageCreate {
  content: string;
  conversationId: number;
  senderId: string;
  senderType: string;
}

export enum ConversationStatus {
  Open = 1,
  Closed = 2,
  Pending = 3,
  Archived = 4,
}

export interface Conversation {
  id: number;
  name?: string;
  status: ConversationStatus;
  customerId: string;
  customerName: string;
  messages?: Message[];
  createdAt: string;
  updatedAt: string;
}

export interface ConversationCreate {
  name?: string;
  customerId: string;
}

export interface ConversationUpdate {
  id: number;
  name?: string;
  status: ConversationStatus;
}
