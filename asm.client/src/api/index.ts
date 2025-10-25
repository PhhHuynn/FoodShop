import axios from "axios";
export default axios.create({
  baseURL: "https://localhost:7108/api",
  timeout: 5000,
});
