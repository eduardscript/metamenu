"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

const routes = [
  {
    href: "/",
    label: "Tenants",
  },
  {
    href: "/tag-categories",
    label: "Tag Categories",
  },
  {
    href: "/tags",
    label: "Tags",
  },
  {
    href: "/products",
    label: "Products",
  },
];

export default function Navbar() {
  const pathname = usePathname();
  const isActive = (path: string) => path === pathname;

  return (
    <nav className="flex items-center justify-between flex-wrap bg-teal-500 p-6">
      <div className="w-full block flex-grow lg:flex lg:items-center lg:w-auto">
        <div className="text-sm lg:flex-grow">
          {routes.map((route) => (
            <Link
              key={route.href}
              href={route.href}
              className={`block mt-4 lg:inline-block lg:mt-0 ${
                isActive(route.href) ? "text-white font-bold" : "text-teal-200"
              } hover:text-white mr-4`}
            >
              {route.label}
            </Link>
          ))}
        </div>
        <div>
          <a className="inline-block text-sm px-4 py-2 leading-none border rounded text-white border-white hover:border-transparent hover:text-teal-500 hover:bg-white mt-4 lg:mt-0">
            0.0.1
          </a>
        </div>
      </div>
    </nav>
  );
}
