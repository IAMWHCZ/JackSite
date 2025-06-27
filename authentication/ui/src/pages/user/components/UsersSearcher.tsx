import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input"
import { Pencil, Search } from "lucide-react"
import { useTranslation } from "react-i18next"

export const UsersSearcher = () => {
    const { t: u } = useTranslation('user');
    const { t } = useTranslation('common');
    return (
        <div className="flex w-full items-center gap-3">
            <div className="relative flex-1">
                <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform text-slate-400 z-10" />
                <Input
                    className="w-full pl-10"
                    placeholder={u('search_placeholder')}
                />
            </div>
            <Button className="flex-shrink-0 w-24 bg-blue-500 hover:bg-blue-600 text-white">
                <Search />
                {u('search_button')}
            </Button>
            <Button className="flex-shrink-0 w-24">
                <Pencil />
                {t('create')}</Button>
        </div>
    )
}
