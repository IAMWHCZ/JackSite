import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from '@/components/ui/dropdown-menu';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { UserStatus } from '@/enums/user';
import { useUserStore } from '@/stores/user';
import { createColumnHelper, flexRender, getCoreRowModel, useReactTable } from '@tanstack/react-table'
import { Ban, Edit, Eye, MoreHorizontal, Trash2 } from 'lucide-react';
import { useCallback, useMemo } from 'react';
import { useTranslation } from 'react-i18next';
export type User = {
    userId: number
    avatar: string
    username: string
    email: string
    status: UserStatus
    createOn: Date
};
export const UsersTabler = () => {
    const { openDetail } = useUserStore()
    const { t } = useTranslation()
    const handleOpenDetail = useCallback((userId: number) => {
        openDetail(userId)
    }, [openDetail])
    const data: User[] = useMemo(() => [
        {
            userId: 1,
            avatar: "https://github.com/shadcn.png",
            username: "shadcn",
            email: "shadcn@example.com",
            status: UserStatus.Active,
            createOn: new Date("2024-01-15")
        },
        {
            userId: 2,
            avatar: "https://github.com/vercel.png",
            username: "vercel",
            email: "vercel@example.com",
            status: UserStatus.Inactive,
            createOn: new Date("2024-01-14")
        },
        {
            userId: 3,
            avatar: "https://github.com/vercel.png",
            username: "vite",
            email: "vite@example.com",
            status: UserStatus.Banned,
            createOn: new Date("2024-01-14")
        }
    ], [])
    const columnHelper = createColumnHelper<User>()
    const columns = useMemo(() => [
        columnHelper.accessor('avatar', {
            header: '头像',
            cell: info => (
                <div className="flex justify-center items-center">
                    <Avatar className="h-10 w-10">
                        <AvatarImage src={info.getValue()} alt={info.row.original.username} />
                        <AvatarFallback>
                            {info.row.original.username.slice(0, 2).toUpperCase()}
                        </AvatarFallback>
                    </Avatar>
                </div>
            ),
        }),
        columnHelper.accessor('username', {
            header: '用户名',
            cell: info => (
                <div className="font-medium">
                    {info.getValue()}
                </div>
            ),
        }),
        columnHelper.accessor('email', {
            header: '邮箱',
            cell: info => (
                <div className="text-muted-foreground">
                    {info.getValue()}
                </div>
            ),
        }),
        columnHelper.accessor('status', {
            header: '状态',
            cell: info => {
                const status = info.getValue()
                return (
                    <Badge variant={
                        status === UserStatus.Active ? 'default' :
                            status === UserStatus.Inactive ? 'secondary' :
                                'destructive'
                    }>
                        {status === UserStatus.Active ? '活跃' :
                            status === UserStatus.Inactive ? '未激活' :
                                '已封禁'}
                    </Badge>
                )
            },
        }),
        columnHelper.accessor('createOn', {
            header: '创建时间',
            cell: info => (
                <div className="text-muted-foreground">
                    {new Date(info.getValue()).toLocaleDateString('zh-CN')}
                </div>
            ),
        }),
        columnHelper.display({
            id: 'actions',
            header: '操作',
            cell: ({ row }) => {
                const user = row.original
                return (
                    <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                            <Button variant="ghost" className="h-8 w-8 p-0">
                                <MoreHorizontal className="h-4 w-4" />
                            </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                            <DropdownMenuItem onSelect={() => handleOpenDetail(user.userId)}>
                                <Eye className="mr-2 h-4 w-4" />
                                {t("view")}
                            </DropdownMenuItem>
                            <DropdownMenuItem>
                                <Edit className="mr-2 h-4 w-4" />
                                {t("edit")}
                            </DropdownMenuItem>
                            <DropdownMenuItem className="text-destructive">
                                <Ban className="mr-2 h-4 w-4 text-destructive" />
                                {t("disable")}
                            </DropdownMenuItem>
                            <DropdownMenuItem className="text-destructive">
                                <Trash2 className="mr-2 h-4 w-4 text-destructive" />
                                {t("delete")}
                            </DropdownMenuItem>
                        </DropdownMenuContent>
                    </DropdownMenu>
                )
            },
        }),
    ], [])
    const table = useReactTable({
        data,
        columns,
        getCoreRowModel: getCoreRowModel(),
    })
    return (
        <div className="w-full">
            <div className="rounded-md border">
                <Table>
                    <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header) => (
                                    <TableHead className='text-center' key={header.id}>
                                        {header.isPlaceholder
                                            ? null
                                            : flexRender(
                                                header.column.columnDef.header,
                                                header.getContext()
                                            )}
                                    </TableHead>
                                ))}
                            </TableRow>
                        ))}
                    </TableHeader>
                    <TableBody>
                        {table.getRowModel().rows?.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    data-state={row.getIsSelected() && "selected"}
                                >
                                    {row.getVisibleCells().map((cell) => (
                                        <TableCell className='text-center' key={cell.id}>
                                            {flexRender(
                                                cell.column.columnDef.cell,
                                                cell.getContext()
                                            )}
                                        </TableCell>
                                    ))}
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    className="h-24 text-center"
                                >
                                    暂无数据
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>
        </div>
    )
}
