import axios from "axios";

const API_URL = "http://localhost:5283/api/auth";

export async function loginApi(username: string, password: string) {
  const res = await axios.post(`${API_URL}/login`, {
    username,
    password,
  });

  return res.data; // { token, message }
}

export async function registerApi(
  username: string,
  password: string,
  firstName: string
) {
  const res = await axios.post(`${API_URL}/register`, {
    username,
    password,
    firstName,
  });

  return res.data; // { token, message }
}
