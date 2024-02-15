"use client";

import Select from "react-select";
import { usePathname, useSearchParams, useRouter } from "next/navigation";
import { Tenant } from "@/server/models/tenant";

export default function SelectTenant({ tenants }: { tenants: Tenant[] }) {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  const tenantCodeQuery = searchParams.get("tenantCode");

  function handleSearch(value: number) {
    const params = new URLSearchParams(searchParams);

    value > 0
      ? params.set("tenantCode", value.toString())
      : params.delete("tenantCode");

    params.delete("tagCategoryCode");

    replace(`${pathname}?${params.toString()}`);
  }

  function clear() {
    replace(`${pathname}`);
  }

  const options = tenants.map((c) => ({
    value: c.code,
    label: `(${c.code}) ${c.name}`,
  }));

  function setValue() {
    if (tenantCodeQuery) {
      return options.find((o) => o.value == parseInt(tenantCodeQuery));
    }

    return null!;
  }

  return (
    <div className="flex gap-2">
      <Select
        className="flex-grow"
        options={options}
        onChange={(p) => handleSearch(p!.value!)}
        placeholder="Select a tenant"
        value={setValue()}
      />
      <button className="btn btn-accent text-white" onClick={() => clear()}>
        Clear filter
      </button>
    </div>
  );
}
