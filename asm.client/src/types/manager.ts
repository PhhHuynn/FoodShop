export interface ColumnConfig {
  key: string;
  label: string;
  type: "text" | "number" | "date";
}

export interface ManagerConfig {
  title: string;
  endpoint: string;
  columns: ColumnConfig[];
}
