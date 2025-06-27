import { useUserStore } from "@/stores/user"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog"

export const UsersDetailer = () => {
    const { isShowDetail, selectedUserId, closeDetail } = useUserStore()
    return (
        <Dialog open={isShowDetail} onOpenChange={(open) => !open && closeDetail()}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>用户详情</DialogTitle>
                    <DialogDescription>
                        查看用户 ID: {selectedUserId} 的详细信息
                    </DialogDescription>
                </DialogHeader>
                <div className="grid gap-4 py-4">
                    {/* 用户详情内容 */}
                    <div className="grid grid-cols-4 items-center gap-4">
                        <span className="text-right">账号:</span>
                        <span className="col-span-3">shadcn</span>
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <span className="text-right">邮箱:</span>
                        <span className="col-span-3">shadcn@example.com</span>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}