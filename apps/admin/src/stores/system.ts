import { create } from 'zustand'

interface Props {
    isSideBarCollapsed: boolean
    setSideBarCollapsed: (isCollapsed: boolean) => void
}
export const useSystemStore = create<Props>((set)=>({
    isSideBarCollapsed: false,
    setSideBarCollapsed: (isCollapsed) => set({isSideBarCollapsed: isCollapsed})
}))

