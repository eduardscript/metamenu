"use client";

import { usePathname, useSearchParams, useRouter } from "next/navigation";

export default function SearchInput() {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  function handleSearch(term: string) {
    const params = new URLSearchParams(searchParams);
    if (term) {
      params.set("tenant", term);
    } else {
      params.delete("tenant");
    }
    replace(`${pathname}?${params.toString()}`);
  }

  return (
    <input
      type="text"
      placeholder="Search by tenant id or name"
      className="rounded px-2 py-4 w-screen"
      onChange={(e) => handleSearch(e.target.value)}
      defaultValue={searchParams.get("tenant")?.toString()}
    />
  );
}
