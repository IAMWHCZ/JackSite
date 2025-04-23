import { useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { Box, Button, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Tooltip } from "@mui/material";
import {
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { Edit2, Trash2 } from "lucide-react";
import type { RouteConfig } from "@/types/gateway";
import { useRoutes } from "@/hooks/useRoutes";
import { TableLoading } from "@/components/loading/TableLoading";
import { useGatewayStore } from "@/stores/gateway";
import { useDialogs } from "@toolpad/core/useDialogs";
import toast from "react-hot-toast";


const fallbackData: RouteConfig[] = [];
// 定义列宽配置
const columnSizes = {
  routeId: 120,        // 减小到 120px
  matchPath: 140,      // 减小到 140px
  order: 60,           // 减小到 60px
  clusterId: 120,      // 减小到 120px
  policy: 100,         // 减小到 100px
  timeout: 80,         // 减小到 80px
  bodySize: 90,        // 减小到 90px
  transforms: 120,     // 减小到 120px
  actions: 80,         // 减小到 80px
} as const;

export const RouteTable = () => {
  const { t } = useTranslation();
  const columnHelper = createColumnHelper<RouteConfig>();
  const { list ,delete:deleteRoute} = useRoutes();
  const dialogs = useDialogs();
  const {setOpenRouteDialog,setSelectedRoute} = useGatewayStore();
  const [routeId, setRouteId] = useState('')
  const onEdit = (id: string) => {
    setSelectedRoute(id);
    setOpenRouteDialog(true);
  }

  const onDelete = async (id: string) => {
    await dialogs.confirm(<div className='flex flex-col'>
      <span className='text-lg font-bold'>{t('gateway.clusters.delete.label')}</span>
      <TextField value={routeId} onChange={(e) => setRouteId(e.target.value)} sx={{ mt: 2 }} id="standard-basic" label={t('gateway.table.clusterId')} variant="standard" />
    </div>, {
      title: <span className='text-xl font-bold'>{t('common.notification')}</span>,
      okText: <Button loading={deleteRoute.isPending} onClick={async () => {
        if (routeId === id) {
          await deleteRoute.mutateAsync(id)
          if (deleteRoute.isSuccess) {
            toast.success(t('common.deleteSuccess'))
          }
        } else {
          if (deleteRoute.isError) {
            toast.error(deleteRoute.error?.message)
          }
        }
      }} color='error'>{t('common.confirm')}</Button>,
      cancelText: t('common.cancel'),
    })
  };
  const columns = useMemo(
    () => [
      columnHelper.accessor("routeId", {
        header: t("gateway.table.routeName"),
        cell: (info) => info.getValue(),
        size: columnSizes.routeId,
      }),
      columnHelper.accessor("match", {
        header: t("gateway.table.matchPath"),
        size: columnSizes.matchPath,
        cell: (info) => {
          const match = info.getValue();
          if (!match) return '-';
          return (
            <Tooltip title={match.path}>
              <div> {/* 使用一个容器元素包裹 */}
                <Button
                  variant="text"
                  sx={{
                    textTransform: 'none',
                    whiteSpace: 'nowrap',
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    width: '100%',
                    justifyContent: 'flex-start',
                    px: 1,
                    minWidth: 'unset',
                  }}
                >
                  {match.path}
                </Button>
              </div>
            </Tooltip>
          );
        },
      }),
      columnHelper.accessor("order", {
        header: t("gateway.table.order"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.order,
      }),
      columnHelper.accessor("clusterId", {
        header: t("gateway.table.targetCluster"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.clusterId,
      }),
      columnHelper.accessor("authorizationPolicy", {
        header: t("gateway.table.authPolicy"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.policy,
      }),
      columnHelper.accessor("rateLimiterPolicy", {
        header: t("gateway.table.rateLimit"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.policy,
      }),
      columnHelper.accessor("timeoutPolicy", {
        header: t("gateway.table.timeoutPolicy"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.policy,
      }),
      columnHelper.accessor("timeout", {
        header: t("gateway.table.timeout"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.timeout,
      }),
      columnHelper.accessor("corsPolicy", {
        header: t("gateway.table.corsPolicy"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.policy,
      }),
      columnHelper.accessor("maxRequestBodySize", {
        header: t("gateway.table.maxBodySize"),
        cell: (info) => info.getValue() ?? '-',
        size: columnSizes.bodySize,
      }),
      columnHelper.accessor("transforms", {
        header: t("gateway.table.transforms"),
        cell: (info) => {
          const transforms = info.getValue();
          if (!transforms) return '-';
          const path = transforms[0].pathPattern;
          return (
            <Button
              variant="text"
              sx={{
                textTransform: 'none',
                whiteSpace: 'nowrap',
                overflow: 'hidden',
                textOverflow: 'ellipsis',
              }}
            >
              {path}
            </Button>
          );
        },
        size: columnSizes.transforms,
      }),
      columnHelper.display({
        id: "actions",
        header: t("gateway.table.actions"),
        cell: (info) => (
          <Box display="flex" justifyContent="center" gap={0.5}>
            <Tooltip title={t("gateway.actions.edit")}>
              <span> {/* 使用 span 包裹 IconButton */}
                <IconButton
                  size="small"
                  onClick={() => onEdit(info.row.original.routeId)}
                >
                  <Edit2 size={16} />
                </IconButton>
              </span>
            </Tooltip>
            <Tooltip title={t("gateway.actions.delete")}>
              <span> {/* 使用 span 包裹 IconButton */}
                <IconButton
                  size="small"
                  onClick={() => onDelete(info.row.original.routeId)}
                >
                  <Trash2 size={16} />
                </IconButton>
              </span>
            </Tooltip>
          </Box>
        ),
        size: columnSizes.actions,
      }),
    ],
    [columnHelper, onEdit, onDelete, t]
  );
  useEffect(()=>{
    console.log('list',list);
    
  },[

  ])
  const table = useReactTable({
    data: list.data?.value.routes ?? fallbackData,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <Paper
      elevation={0} // 移除默认阴影
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        overflow: 'hidden',
        border: '1px solid',
        borderColor: 'divider',
        borderRadius: 2, // 更大的圆角
        bgcolor: 'background.paper',
        maxWidth: '100%', // 确保不超过父容器
      }}
    >
      <Box
        sx={{
          flexGrow: 1,
          overflow: 'auto',
          maxWidth: '100%', // 确保不超过父容器
        }}
      >
        <TableContainer
          sx={{
            maxWidth: '100%', // 确保不超过父容器
            width: 'fit-content', // 让容器适应内容宽度
            minWidth: '100%', // 但至少要和父容器一样宽
          }}
        >
          <Table
            stickyHeader
            sx={{
              width: '100%',
              tableLayout: 'fixed',
              '& .MuiTableCell-root': {
                px: 1, // 减小单元格的水平内边距
              }
            }}
          >
            <TableHead>
              {table.getHeaderGroups().map((headerGroup) => (
                <TableRow key={headerGroup.id}>
                  {headerGroup.headers.map((header) => (
                    <TableCell
                      key={header.id}
                      align="center"
                      sx={{
                        width: header.column.getSize(),
                        whiteSpace: 'nowrap',
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        padding: '16px',
                        backgroundColor: (theme) =>
                          theme.palette.mode === 'dark'
                            ? 'background.paper'
                            : 'background.default',
                        fontWeight: 600,
                        fontSize: '0.875rem',
                        letterSpacing: '0.01em',
                        color: 'text.primary',
                        position: 'sticky',
                        top: 0,
                        zIndex: 2,
                        borderBottom: '2px solid',
                        borderColor: 'divider',
                        transition: 'all 0.2s ease',
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
                      transition: 'all 0.2s ease',
                      '&:hover': {
                        backgroundColor: (theme) =>
                          theme.palette.mode === 'dark'
                            ? 'rgba(255, 255, 255, 0.05)'
                            : 'rgba(0, 0, 0, 0.02)',
                        '& .MuiTableCell-root': {
                          color: 'text.primary',
                        },
                      },
                    }}
                  >
                    {row.getVisibleCells().map((cell) => (
                      <TableCell
                        key={cell.id}
                        align="center"
                        sx={{
                          overflow: 'hidden',
                          textOverflow: 'ellipsis',
                          padding: '12px 16px',
                          height: '52px',
                          fontSize: '0.875rem',
                          color: 'text.secondary',
                          borderBottom: '1px solid',
                          borderColor: 'divider',
                          transition: 'all 0.2s ease',
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











