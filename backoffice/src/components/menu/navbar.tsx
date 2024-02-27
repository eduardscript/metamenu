'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'

const routes = [
  {
    href: '/',
    label: 'Tenants',
  },
  {
    href: '/tag-categories',
    label: 'Tag Categories',
  },
  {
    href: '/tags',
    label: 'Tags',
  },
  {
    href: '/products',
    label: 'Products',
  },
]

export default function Navbar() {
  const pathname = usePathname()
  const isActive = (path: string) => path === pathname

  return (
    <div className="navbar bg-black">
      <div className="navbar-start">
        <div className="dropdown">
          <div tabIndex={0} role="button" className="btn btn-ghost btn-circle">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              className="h-5 w-5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M4 6h16M4 12h16M4 18h7"
              />
            </svg>
          </div>
          <ul
            tabIndex={0}
            className="menu menu-sm dropdown-content mt-3 z-[1] p-2 shadow bg-base-100 rounded-box w-52"
          >
            {routes.map((route) => (
              <li>
                <Link
                  key={route.href}
                  href={route.href}
                  className={`block mt-4 lg:inline-block lg:mt-0 ${
                    isActive(route.href)
                      ? 'text-white font-bold'
                      : 'text-gray-200 text-xs'
                  } hover:text-white mr-4`}
                >
                  {route.label}
                </Link>
              </li>
            ))}
          </ul>
        </div>
      </div>
      <div className="navbar-center">
        <a className="btn btn-ghost text-xl">META MENU</a>
      </div>
      <div className="navbar-end">
        <div className="dropdown dropdown-end">
          <div
            tabIndex={0}
            role="button"
            className="btn btn-ghost btn-circle avatar"
          >
            <div className="w-10 rounded-full">
              <img
                alt="Tailwind CSS Navbar component"
                src="https://daisyui.com/images/stock/photo-1534528741775-53994a69daeb.jpg"
              />
            </div>
          </div>
          <ul
            tabIndex={0}
            className="menu menu-sm dropdown-content mt-3 z-[1] p-2 shadow bg-base-100 rounded-box w-52"
          >
            <li>
              <a className="justify-between">
                Profile
                <span className="badge">New</span>
              </a>
            </li>
            <li>
              <a>Settings</a>
            </li>
            <li>
              <a>Logout</a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  )
}
