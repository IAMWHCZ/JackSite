import http from "@/lib/http"
export const LoginByPassword = async () => {
  const response =  await http.post<>('users');
  return response;
}