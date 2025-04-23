import {Link} from "@tanstack/react-router";
import {cn} from "@/lib/utils";
import {NavItem} from "@/config/navigation";
import {
    NavigationMenuTrigger,
    NavigationMenu,
    NavigationMenuContent,
    NavigationMenuItem,
    NavigationMenuLink,
    NavigationMenuList
} from "./NavigationMenu";

interface NavDropdownProps {
    item: NavItem;
    parentPath?: string;
}

const NestedDropdown = ({ item }: { item: NavItem; parentPath?: string }) => {
    return (
        <ul className="grid w-[200px] gap-1 p-2">
            {item.children?.map((child) => (
                <li key={child.title}>
                    {child.children ? (
                        <NestedDropdown item={child} />
                    ) : (
                        <NavigationMenuLink asChild>
                            <Link
                                to={item.href}
                                className={cn(
                                    "block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground",
                                    child.disabled && "pointer-events-none opacity-50"
                                )}
                            >
                                <div className="text-sm font-medium leading-none">
                                    {child.title}
                                </div>
                            </Link>
                        </NavigationMenuLink>
                    )}
                </li>
            ))}
        </ul>
    );
};

export function NavDropdown({item}: NavDropdownProps) {
    
    return (
        <NavigationMenu className="relative">
            <NavigationMenuList className="justify-start">
                <NavigationMenuItem>
                    <NavigationMenuTrigger
                        className={cn(
                            "text-sm font-medium transition-colors hover:text-primary",
                            item.disabled && "pointer-events-none opacity-50"
                        )}
                    >
                        {item.title}
                    </NavigationMenuTrigger>
                    <NavigationMenuContent>
                        <ul className="grid w-[200px] gap-1 p-2">
                            {item.children?.map((child) => (
                                <li key={child.title}>
                                    {child.children ? (
                                        <NestedDropdown item={child} />
                                    ) : (
                                        <NavigationMenuLink asChild>
                                            <Link
                                                to={child.href}
                                                className={cn(
                                                    "block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground",
                                                    child.disabled && "pointer-events-none opacity-50"
                                                )}
                                            >
                                                <div className="text-sm font-medium leading-none">
                                                    {child.title}
                                                </div>
                                            </Link>
                                        </NavigationMenuLink>
                                    )}
                                </li>
                            ))}
                        </ul>
                    </NavigationMenuContent>
                </NavigationMenuItem>
            </NavigationMenuList>
        </NavigationMenu>
    );
}
