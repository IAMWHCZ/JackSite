import { create } from 'zustand';

interface UserStore {
  isShowDetail: boolean;
  selectedUserId: number | null;
  openDetail: (userId: number) => void;
  closeDetail: () => void;
}

export const useUserStore = create<UserStore>((set, get) => ({
  isShowDetail: false,
  selectedUserId: null,
  openDetail: userId => {
    const { selectedUserId: currentUserId, isShowDetail } = get();
    // 避免重复设置相同的状态
    if (currentUserId !== userId || !isShowDetail) {
      set({ isShowDetail: true, selectedUserId: userId });
    }
  },
  closeDetail: () => {
    const { isShowDetail } = get();
    if (isShowDetail) {
      set({ isShowDetail: false, selectedUserId: null });
    }
  },
}));
