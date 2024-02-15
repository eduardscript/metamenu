import React from "react";

interface Entity {
  code: number | string;
  name?: string;
}

interface EntityCardProps {
  actions: React.ReactNode;
  additionalActions?: React.ReactNode;
  entity: Entity;
}

const EntityCard: React.FC<EntityCardProps> = ({
  actions,
  additionalActions,
  entity,
}) => {
  const { name, code } = entity;

  return (
    <div className="bg-white border rounded-xl p-5">
      <div className="flex justify-between">
        {name && <p className="font-bold">{name}</p>}
        <p className="text-gray-500">({code})</p>
      </div>
      <div className="flex justify-between mt-2">
        <div className="flex flex-col gap-2">
          <div className="flex gap-2">{actions}</div>
          {additionalActions}
        </div>
      </div>
    </div>
  );
};

export default EntityCard;
