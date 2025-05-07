import {ThemeToggle} from "@/components/theme/ThemeToggle";
import {LoginModal} from "@/components/login/LoginModal";
import {useAuthorizationStore} from "@/stores/modules/auth";
import {navigationConfig} from "@/config/navigation";

import {cn} from "@/lib/utils";
import {NavLink} from "@/components/core/NavLink.tsx";
import {NavDropdown} from "@/components/core/NavDropdown";
import {Logo} from "@/components/core/Logo";

interface NavbarProps {
    logoTitle?: string;
    logoImage?: string;
    showThemeToggle?: boolean;
    showLogin?: boolean;
    className?: string;
}

export const Navbar = ({
                           logoTitle = "CZ WebTools",
                           logoImage = "/logo.png",
                           showThemeToggle = true,
                           showLogin = true,
                           className,
                       }: NavbarProps) => {
    const {isOpen, setIsOpen} = useAuthorizationStore();
    return (
        <nav
            className={cn(
                "sticky top-0 z-50 w-full border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60",
                className
            )}
        >
            <div className="px-4 sm:px-6 lg:px-8 flex h-14 items-center justify-between">
                {/* Logo and Brand */}
                <Logo title={logoTitle} imagePath={logoImage}/>

                {/* Main Navigation */}
                <div className="flex items-center space-x-6  w-2/3">
                    <nav className="flex items-center space-x-6">
                        {navigationConfig.map((item) => (
                            item.children ? (
                                <NavDropdown
                                    key={item.title}
                                    item={item}
                                />
                            ) : (
                                <NavLink
                                    key={item.title}
                                    href={item.href || ""}
                                    title={item.title}
                                    disabled={item.disabled}
                                    external={item.external}
                                />
                            )
                        ))}
                    </nav>
                </div>

                {/* Right side items */}
                <div className="flex items-center space-x-4">
                    {showThemeToggle && <ThemeToggle/>}
                    {showLogin && (
                        <LoginModal
                            open={isOpen}
                            onOpenChange={() => setIsOpen(!isOpen)}
                        />
                    )}
                </div>
            </div>
        </nav>
    );
};