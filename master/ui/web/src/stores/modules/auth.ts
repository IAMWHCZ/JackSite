import {create} from "zustand";

export interface IAuthorizationState {
    isOpen: boolean;
    setIsOpen: (isOpen: boolean) => void;
}

export const useAuthorizationStore = create<IAuthorizationState>((set) => ({
    isOpen: false,
    setIsOpen: (isOpen) => set({isOpen}),
}));