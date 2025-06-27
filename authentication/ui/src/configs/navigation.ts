import type { MouseEventHandler } from 'react';

export type NavigationItemProp = {
  key: number | string;
  label: string;
  to: string;
  click?: MouseEventHandler<HTMLAnchorElement> | undefined;
  isSelected: boolean;
};
export const NavigationItems: NavigationItemProp[] = [
  {
    key: 1,
    label: 'navigation.home',
    to: '/',
    isSelected: true,
    click: () => {},
  },
  {
    key: 2,
    label: 'navigation.account',
    to: '/user',
    isSelected: false,
  },
  {
    key: 3,
    label: 'navigation.resource',
    to: '/',
    isSelected: false,
  },
  {
    key: 4,
    label: 'navigation.authorization',
    to: '/',
    isSelected: false,
  },
  {
    key: 5,
    label: 'navigation.user-group',
    to: '/',
    isSelected: false,
  },
];
