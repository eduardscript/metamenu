import { FC } from 'react';

interface TagCategoryCardProps {
  code: string;
  onEdit?: () => void;
  onDelete?: () => void;
}

const TagCategoryCard: FC<TagCategoryCardProps> = ({ code, onEdit, onDelete }) => {
  return (
    <div className="bg-white p-4 mb-4 rounded-md shadow-md">
      <h3 className="text-lg font-bold mb-2">{code}</h3>

      <div className="flex justify-end">
        {onEdit && (
          <button className="text-blue-500 hover:text-blue-700 mr-2" onClick={onEdit}>
            Edit
          </button>
        )}

        {onDelete && (
          <button className="text-red-500 hover:text-red-700" onClick={onDelete}>
            Delete
          </button>
        )}
      </div>
    </div>
  );
};

export default TagCategoryCard;
