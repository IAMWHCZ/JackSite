import { Link } from "@tanstack/react-router";
import { cn } from "@/lib/utils";

interface LogoProps {
    title?: string;
    imagePath?: string;
    className?: string;
}

export function Logo({ title, imagePath, className }: LogoProps) {
    return (
        <Link
            to="/"
            className={cn(
                "flex items-center gap-2 font-medium transition-colors hover:text-primary",
                className
            )}
        >
            {imagePath && (
                <img
                    src={imagePath}
                    alt={title || "Logo"}
                    className="h-8 w-8 object-contain"
                />
            )}
            {title && <span className="text-lg font-semibold">{title}</span>}
        </Link>
    );
}