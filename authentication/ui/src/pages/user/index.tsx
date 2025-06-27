import { useTranslation } from "react-i18next";
import { UsersDetailer } from "./components/UsersDetailer"
import { UsersSearcher } from "./components/UsersSearcher"
import { UsersTabler } from "./components/UsersTabler"

export const UserPage = () => {
    const { t } = useTranslation('user');
    return (
        <div className="flex flex-col gap-4 p-4 w-full h-full justify-center items-center">
            <h1 className="text-2xl font-bold">{t("user_management")}</h1>
            <UsersSearcher />
            <UsersTabler />
            <UsersDetailer />
        </div>
    )
}
