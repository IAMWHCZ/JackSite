import { Link } from "@tanstack/react-router";
import { cn } from "@/lib/utils";

interface NavLinkProps {
    href: string;
    title: string;
    className?: string;
    active?: boolean;
    disabled?: boolean;
    external?: boolean;
    onClick?: () => void;
}

export function NavLink({
                            href,
                            title,
                            className,
                            active,
                            disabled,
                            external,
                            onClick,
                        }: NavLinkProps) {
    const classes = cn(
        "text-sm font-medium transition-colors hover:text-primary",
        active && "text-primary",
        disabled && "pointer-events-none opacity-50",
        className
    );
    if (external) {
        return (
            <a
                href={href}
                className={classes}
                target="_blank"
                rel="noopener noreferrer"
                onClick={onClick}
            >
                {title}
            </a>
        );
    }

    return (
        <Link
            to={href}
            className={classes}
            onClick={onClick}
            disabled={disabled}
        >
            {title}
        </Link>
    );
}