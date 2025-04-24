import { useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import {
  Box,
  Button,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Tooltip,
} from "@mui/material";
import {
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { Edit2, Trash2 } from "lucide-react";
import type { ClusterConfig } from "@/types/gateway";
import { useClusters } from "@/hooks/useClusters";
import { TableLoading } from "@/components/loading/TableLoading";
import { useGatewayStore } from "@/stores/gateway";
import toast from "react-hot-toast";
import { useDialogs } from "@toolpad/core/useDialogs";
const fallbackData: ClusterConfig[] = [];
const columnHelper = createColumnHelper<ClusterConfig>();

// 定义列宽配置
const columnSizes = {
  clusterId: 180, // 集群ID，重要标识信息
  policy: 150, // 各种策略列的默认宽度
  status: 120, // 启用/禁用状态列
  destinations: 200, // 目标地址列，需要较大宽度显示地址
  actions: 100, // 操作按钮列
} as const;

export const ClusterTable = () => {
  const { t } = useTranslation();
  const { list } = useClusters();
  const { setOpenClusterDialog, setSelectedCluster } = useGatewayStore();
  const [clusterId, setClusterId] = useState("");
  const { delete: deleteCluster } = useClusters();
  const dialogs = useDialogs();
  const onEdit = (data: ClusterConfig) => {
    setSelectedCluster(data);
    setOpenClusterDialog(true);
  };

  const onDelete = async (id: string) => {
    await dialogs.confirm(
      <div className="flex flex-col">
        <span className="text-lg font-bold">
          {t("gateway.clusters.delete.label")}
        </span>
        <TextField
          value={clusterId}
          onChange={(e) => setClusterId(e.target.value)}
          sx={{ mt: 2 }}
          id="standard-basic"
          label={t("gateway.table.clusterId")}
          variant="standard"
        />
      </div>,
      {
        title: (
          <span className="text-xl font-bold">{t("common.notification")}</span>
        ),
        okText: (
          <Button
            loading={deleteCluster.isPending}
            onClick={async () => {
              if (clusterId === id) {
                await deleteCluster.mutateAsync(id);
                if (deleteCluster.isSuccess) {
                  toast.success(t("common.deleteSuccess"));
                }
              } else {
                if (deleteCluster.isError) {
                  toast.error(deleteCluster.error?.message);
                }
              }
            }}
            color="error"
          >
            {t("common.confirm")}
          </Button>
        ),
        cancelText: t("common.cancel"),
      }
    );
  };

  const columns = useMemo(
    () => [
      columnHelper.accessor("clusterId", {
        header: t("gateway.table.clusterId"),
        cell: (info) => info.getValue(),
        size: columnSizes.clusterId,
      }),
      columnHelper.accessor("loadBalancingPolicy", {
        header: t("gateway.table.loadBalancing"),
        cell: (info) => info.getValue() || "-",
        size: columnSizes.policy,
      }),
      columnHelper.accessor("sessionAffinity", {
        header: t("gateway.table.sessionAffinity"),
        cell: (info) => {
          const sessionAffinity = info.getValue();
          if (!sessionAffinity) return t("common.disabled");
          return (
            <Button>
              {sessionAffinity.enabled
                ? t("common.enabled")
                : t("common.disabled")}
            </Button>
          );
        },
        size: columnSizes.status,
      }),
      columnHelper.accessor("healthCheck", {
        header: t("gateway.table.healthCheck"),
        cell: (info) => {
          const healthCheck = info.getValue();
          if (!healthCheck) return t("common.disabled");
          return (
            <Button>
              {healthCheck.active?.enabled
                ? t("common.enabled")
                : t("common.disabled")}
            </Button>
          );
        },
        size: columnSizes.status,
      }),
      columnHelper.accessor("httpClient", {
        header: t("gateway.table.httpClient"),
        cell: (info) => {
          const httpClient = info.getValue();
          if (!httpClient) return t("common.disabled");
          return (
            <Button>
              {httpClient.dangerousAcceptAnyServerCertificate
                ? t("common.enabled")
                : t("common.disabled")}
            </Button>
          );
        },
        size: columnSizes.status,
      }),
      columnHelper.accessor("httpRequest", {
        header: t("gateway.table.httpRequest"),
        cell: (info) => {
          const request = info.getValue();
          if (!request) return t("common.disabled");
          return (
            <Button>{request ? request.version : t("common.disabled")}</Button>
          );
        },
        size: columnSizes.status,
      }),
      columnHelper.accessor("destinations", {
        header: t("gateway.table.destinations"),
        cell: (info) => {
          const destinations = info.getValue();
          return destinations
            ? Object.entries(destinations).map(([key, dest]) => (
                <div key={key}>{dest.address}</div>
              ))
            : "-";
        },
        size: columnSizes.destinations,
      }),
      columnHelper.accessor("metadata", {
        header: t("gateway.table.metadata"),
        cell: (info) => {
          const metadata = info.getValue();
          return metadata
            ? Object.keys(metadata).length > 0
              ? t("common.enabled")
              : t("common.disabled")
            : t("common.disabled");
        },
        size: columnSizes.status,
      }),
      columnHelper.display({
        id: "actions",
        header: t("gateway.table.actions"),
        cell: (info) => (
          <Box display="flex" justifyContent="center" gap={1}>
            <Tooltip title={t("gateway.actions.edit")}>
              <IconButton
                size="small"
                onClick={() => {
                  const allRows = info.table.getCoreRowModel().rows;
                  const rowsData = allRows.map((row) => row.original);
                  const config = rowsData.find(x=>x.clusterId == info.row.original.clusterId)!
                  onEdit(config);
                }}
              >
                <Edit2 size={18} />
              </IconButton>
            </Tooltip>
            <Tooltip title={t("gateway.actions.delete")}>
              <IconButton
                size="small"
                onClick={() => onDelete(info.row.original.clusterId)}
              >
                <Trash2 size={18} />
              </IconButton>
            </Tooltip>
          </Box>
        ),
        size: columnSizes.actions,
      }),
    ],
    [columnHelper, onEdit, onDelete, t]
  );

  const table = useReactTable({
    data: list.data?.value.cluster ?? fallbackData,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <Paper
      elevation={0}
      sx={{
        height: "100%",
        display: "flex",
        flexDirection: "column",
        overflow: "hidden",
        border: "1px solid",
        borderColor: "divider",
        borderRadius: 2,
        bgcolor: "background.paper",
      }}
    >
      <Box sx={{ flexGrow: 1, overflow: "auto" }}>
        <TableContainer>
          <Table
            stickyHeader
            sx={{
              tableLayout: "fixed",
              minWidth: 1200,
            }}
          >
            <TableHead>
              {table.getHeaderGroups().map((headerGroup) => (
                <TableRow key={headerGroup.id}>
                  {headerGroup.headers.map((header) => (
                    <TableCell
                      key={header.id}
                      align="center" // 表头居中
                      sx={{
                        width: header.column.getSize(),
                        whiteSpace: "nowrap",
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                        padding: "16px",
                        backgroundColor: (theme) =>
                          theme.palette.mode === "dark"
                            ? "background.paper"
                            : "background.default",
                        fontWeight: 600,
                        fontSize: "0.875rem",
                        borderBottom: "2px solid",
                        borderColor: "divider",
                      }}
                    >
                      {flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                    </TableCell>
                  ))}
                </TableRow>
              ))}
            </TableHead>
            {list.isLoading ? (
              <TableLoading />
            ) : (
              <TableBody>
                {table.getRowModel().rows.map((row) => (
                  <TableRow
                    key={row.id}
                    sx={{
                      "&:hover": {
                        backgroundColor: "action.hover",
                      },
                    }}
                  >
                    {row.getVisibleCells().map((cell) => (
                      <TableCell
                        key={cell.id}
                        align="center" // 单元格内容居中
                        sx={{
                          overflow: "hidden",
                          textOverflow: "ellipsis",
                          padding: "12px 16px",
                          fontSize: "0.875rem",
                          // 对于包含多行内容的单元格(如destinations)，保持文本左对齐但容器居中
                          "& > div": {
                            textAlign: "center",
                            "& > *": {
                              justifyContent: "center",
                            },
                          },
                        }}
                      >
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
              </TableBody>
            )}
          </Table>
        </TableContainer>
      </Box>
    </Paper>
  );
};
