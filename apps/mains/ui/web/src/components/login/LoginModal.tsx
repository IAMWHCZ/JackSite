import {Button} from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import {Input} from "@/components/ui/input"
import {Label} from "@/components/ui/label"
import {useState} from "react";
import {useMutation} from "@tanstack/react-query";
import {useForm} from "@tanstack/react-form";
import {z} from "zod";

interface LoginModalProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
}

export function LoginModal({open, onOpenChange}: LoginModalProps) {
    const [showPassword, setShowPassword] = useState(false);
    const {isPending} = useMutation({
        mutationKey: ['login'],
        mutationFn: () => {
        }
    });

    // 定义表单验证 schemauseAppForm
    const loginSchema = z.object({
        email: z.string()
            .email('请输入有效的邮箱地址')
            .min(1, '邮箱不能为空'),
        password: z.string()
            .min(6, '密码至少需要6个字符')
            .max(50, '密码不能超过50个字符'),
        rememberMe: z.boolean().default(false),
    });


    const {Field, handleSubmit, state} = useForm({
        defaultValues: {
            email: '',
            password: '',
            rememberMe: false,
        },
        onSubmit: async ({value}) => {
            try {
                console.log('Form submitted:', value);
            } catch (error) {
                console.error('Login failed:', error);
            } finally {
            }
        },
    });

    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button variant="outline">Edit Profile</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Edit profile</DialogTitle>
                    <DialogDescription>
                        Make changes to your profile here. Click save when you're done.
                    </DialogDescription>
                </DialogHeader>
                <div className="grid gap-4 py-4">
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="name" className="text-right">
                            Name
                        </Label>
                        <Input id="name" value="Pedro Duarte" className="col-span-3"/>
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="username" className="text-right">
                            Username
                        </Label>
                        <Input id="username" value="@peduarte" className="col-span-3"/>
                    </div>
                </div>
                <DialogFooter>
                    <Button type="submit">Save changes</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}