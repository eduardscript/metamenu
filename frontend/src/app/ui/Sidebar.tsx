"use client";

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import clxs from 'clsx';

const links = [
    { href: '/dashboard/tenants', label: 'Tenants' },
    { href: '/dashboard/tagcategories', label: 'Tag categories' },
    { href: '/dashboard/tags', label: 'Tags' },
    { href: '/dashboard/products', label: 'Products' },
];

const Sidebar: React.FC = () => {
  const pathName = usePathname();

  return (
    <div className="bg-gray-800 text-white h-screen w-1/5 p-4">
      <h1 className="text-2xl font-bold mb-4">Dashboard</h1>
      <nav>
        <ul>
          {links.map((link) => (
            <li key={link.label} className={clxs(
              'mb-2 p-2 rounded',
              {
                'text-blue-500': pathName === link.href,
                'bg-gray-900': pathName === link.href,
                'hover:text-blue-500': pathName !== link.href,
                'text-white': pathName === link.href,
              }
            )}>
              <Link href={link.href}>
                {link.label}
              </Link>
            </li>
          ))}
        </ul>
      </nav>
    </div>
  );
};

export default Sidebar;
