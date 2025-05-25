import { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import { useTranslation } from 'react-i18next';

interface TitleContextType {
  title: string;
  setTitle: (title: string) => void;
  resetTitle: () => void;
}

const TitleContext = createContext<TitleContextType | undefined>(undefined);

interface TitleProviderProps {
  children: ReactNode;
  defaultTitle?: string;
}

export const TitleProvider = ({
  children,
  defaultTitle = 'JackSite Authentication System',
}: TitleProviderProps) => {
  const { t, i18n } = useTranslation('common');
  const [title, setTitle] = useState<string>(defaultTitle);

  // 当语言改变时，如果标题是默认标题，则更新标题
  useEffect(() => {
    document.title = title;
  }, [title]);

  // 当语言改变时，如果标题是默认标题，则更新标题
  useEffect(() => {
    if (title === defaultTitle) {
      const translatedDefault = t('appTitle', defaultTitle);
      setTitle(translatedDefault);
      document.title = translatedDefault;
    }
  }, [i18n.language, defaultTitle, t, title]);

  const resetTitle = () => {
    const translatedDefault = t('appTitle', defaultTitle);
    setTitle(translatedDefault);
  };

  return (
    <TitleContext.Provider value={{ title, setTitle, resetTitle }}>
      {children}
    </TitleContext.Provider>
  );
};

export const useTitle = () => {
  const context = useContext(TitleContext);
  if (context === undefined) {
    throw new Error('useTitle must be used within a TitleProvider');
  }
  return context;
};

// 页面标题组件，用于设置页面标题
interface PageTitleProps {
  title: string;
  children?: ReactNode;
}

export const PageTitle = ({ title, children }: PageTitleProps) => {
  const { setTitle } = useTitle();

  useEffect(() => {
    setTitle(title);
    return () => {
      // 组件卸载时不重置标题，因为新页面会设置自己的标题
    };
  }, [title, setTitle]);

  return <>{children}</>;
};
