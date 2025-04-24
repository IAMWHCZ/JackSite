export interface ApiResponse<T> {
  isSuccess: boolean;
  error?: string;
  value?: T;
}