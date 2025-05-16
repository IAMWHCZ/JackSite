import { SxProps, Theme } from "@mui/material"
import { LogOut, LucideProps, UserCog, Wrench } from "lucide-react"
import { ForwardRefExoticComponent, RefAttributes } from "react"

interface MenuItemProps {
    sx?: SxProps<Theme> | undefined
    icon?: ForwardRefExoticComponent<Omit<LucideProps, "ref"> & RefAttributes<SVGSVGElement>>
    labelKey: string
    onPress: () => void
}
export const menuItems: MenuItemProps[] = [
    {
        labelKey: 'menu.profile',
        onPress: () => {
            console.log('个人中心')
        },
        icon: UserCog
    },
    {
        labelKey: 'menu.settings',
        onPress: () => {
            console.log('系统设置')
        },
        icon: Wrench
    },
    {
        labelKey: 'menu.logout',
        onPress: () => {
            console.log('退出登录')
        },
        icon: LogOut
    }
]
