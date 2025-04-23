import { Outlet } from "@tanstack/react-router";
import { Navbar } from "./Navbar";

const RootLayout = () => {
  return (
    <div className="flex min-h-screen w-full justify-center items-center  flex-col bg-background">
      <Navbar />
      <main className="flex-1 container w-full py-8">
        <Outlet />
      </main>
    </div>
  );
};

export default RootLayout;