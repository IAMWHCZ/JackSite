import { http } from "@/lib/http";
import type {
  RouteConfig,
  ClusterConfig,
  RequestLog,
  CreateClusterRequest,
} from "@/types/gateway";

export class GatewayApi {
  private static BASE_PATH = "/api/gateway-config";

  static routes = {
    getList: async () => {
      const response = http.get(`${this.BASE_PATH}/list/1`);
      return response;
    },
    create: (data: RouteConfig) => {
      return http.post<void>(`${this.BASE_PATH}/routes`, data);
    },
    update: (data: RouteConfig) => {
      return http.put<void>(`${this.BASE_PATH}/routes/${data.routeId}`, data);
    },
    delete: (routeId: string) => {
      return http.delete<void>(`${this.BASE_PATH}/routes/${routeId}`);
    },
    getJson: async () => {
      const response = await http.get(`${this.BASE_PATH}/route`);
      return response;
    },
    updateJson: (data: string) => {
      return http.post<void>(`${this.BASE_PATH}/route/json`, data, {
        headers: {
          "Content-Type": "application/json",
        },
      });
    },
  };
  static clusters = {
    getList: async () => {
      const response = await http.get(`${this.BASE_PATH}/list/2`);
      return response;
    },
    create: (data: CreateClusterRequest) => {
      return http.post<void>(`${this.BASE_PATH}/cluster`, data);
    },
    update: (data: ClusterConfig) => {
      return http.put<void>(
        `${this.BASE_PATH}/clusters${data.clusterId}`,
        data
      );
    },
    delete: (clusterId: string) => {
      return http.delete<void>(`${this.BASE_PATH}/cluster/${clusterId}`);
    },
    getJson: async () => {
      const response = await http.get(`${this.BASE_PATH}/cluster`);
      return response;
    },
    updateJson: (data: string) => {
      return http.post<void>(`${this.BASE_PATH}/cluster/json`, data, {
        headers: {
          "Content-Type": "application/json",
        },
      });
    },
  };
  static reload = (type: number) => {
    return http.get<[]>(`${this.BASE_PATH}/reload/${type}`);
  };
  static accessRecords = () => {
    return http.get<RequestLog[]>(`${this.BASE_PATH}/access-records`);
  };
}
