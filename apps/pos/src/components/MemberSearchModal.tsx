import React, { useMemo } from 'react';
import { Modal } from '@coffee-pos/ui';
import { BotanicalSprig, PosIcon } from './PosIcon';
import './MemberSearchModal.css';

export interface MemberRecord {
  id: string;
  name: string;
  phone: string;
  tier: string;
  points: number;
}

export interface MemberSearchModalProps {
  isOpen: boolean;
  query: string;
  members: MemberRecord[];
  attachedMemberId?: string | null;
  onQueryChange: (query: string) => void;
  onClose: () => void;
  onAttach: (member: MemberRecord) => void;
  onRedeem: (member: MemberRecord) => void;
  onDetach?: () => void;
}

const MemberCrown: React.FC = () => (
  <svg className="rl-member-result__crown" viewBox="0 0 48 48" fill="none" aria-hidden="true" focusable="false">
    <path d="M10 18.5 15.5 24l8.5-11 8.5 11 5.5-5.5-2.5 16H12.5L10 18.5Z" stroke="currentColor" strokeWidth="1.6" strokeLinejoin="round" />
    <path d="M13.5 29.5h21M16 36.5h16" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" />
    <circle cx="15.5" cy="16.5" r="2" stroke="currentColor" strokeWidth="1.6" />
    <circle cx="24" cy="11.5" r="2" stroke="currentColor" strokeWidth="1.6" />
    <circle cx="32.5" cy="16.5" r="2" stroke="currentColor" strokeWidth="1.6" />
  </svg>
);

const PointsStar: React.FC = () => (
  <svg className="rl-member-points__star" viewBox="0 0 20 20" fill="none" aria-hidden="true" focusable="false">
    <path d="m10 2.7 2.1 4.3 4.8.7-3.5 3.4.8 4.8-4.2-2.3-4.2 2.3.8-4.8-3.5-3.4 4.8-.7L10 2.7Z" fill="currentColor" stroke="currentColor" strokeWidth="1.1" strokeLinejoin="round" />
  </svg>
);

export const MemberSearchModal: React.FC<MemberSearchModalProps> = ({
  isOpen,
  query,
  members,
  attachedMemberId,
  onQueryChange,
  onClose,
  onAttach,
  onRedeem,
  onDetach
}) => {
  const normalizedQuery = query.trim().toLowerCase();
  const filteredMembers = useMemo(() => members.filter(member => (
    !normalizedQuery
      || member.name.toLowerCase().includes(normalizedQuery)
      || member.phone.includes(normalizedQuery)
  )), [members, normalizedQuery]);

  const footer = (
    <div className="rl-member-search-modal__footer-actions">
      {attachedMemberId && onDetach && (
        <button type="button" className="rl-member-search-modal__detach" onClick={onDetach}>
          Detach Customer
        </button>
      )}
      <button type="button" className="rl-member-search-modal__close-action" onClick={onClose}>
        Close
      </button>
    </div>
  );

  return (
    <Modal
      isOpen={isOpen}
      title="Coffee Club Member Search"
      onClose={onClose}
      maxWidth="lg"
      className="rl-member-search-modal"
      footer={footer}
    >
      <div className="rl-member-search-modal__content">
        <BotanicalSprig className="rl-member-search-modal__sprig rl-member-search-modal__sprig--top" />
        <BotanicalSprig className="rl-member-search-modal__sprig rl-member-search-modal__sprig--bottom" />

        <label className="rl-member-search-modal__search-label" htmlFor="member-search-input">
          Search by phone number or name
        </label>
        <div className="rl-member-search-modal__search-field">
          <PosIcon name="search" size={32} strokeWidth={1.55} />
          <input
            id="member-search-input"
            type="search"
            value={query}
            onChange={event => onQueryChange(event.target.value)}
            placeholder="e.g. +60123456789 or Sarah"
            autoFocus
          />
        </div>

        <div className="rl-member-search-modal__results" aria-live="polite">
          {filteredMembers.length === 0 ? (
            <div className="rl-member-search-modal__empty">
              <PosIcon name="search" size={30} strokeWidth={1.45} />
              <strong>No members found</strong>
              <span>Try another name or phone number.</span>
            </div>
          ) : filteredMembers.map(member => {
            const selected = attachedMemberId === member.id;
            return (
              <article key={member.id} className={`rl-member-result ${selected ? 'is-selected' : ''}`}>
                <div className="rl-member-result__identity">
                  <span className="rl-member-result__avatar"><MemberCrown /></span>
                  <div className="rl-member-result__copy">
                    <h3>{member.name}</h3>
                    <div className="rl-member-result__meta">
                      <span>{member.phone}</span>
                      <span aria-hidden="true">•</span>
                      <span className="rl-member-points"><PointsStar />{member.points} points</span>
                    </div>
                  </div>
                </div>
                <div className="rl-member-result__actions">
                  <button type="button" className="rl-member-result__attach" onClick={() => onAttach(member)}>
                    {selected ? 'Attached' : 'Attach'}
                  </button>
                  {member.points >= 100 && (
                    <button type="button" className="rl-member-result__redeem" aria-label="Redeem RM 10" onClick={() => onRedeem(member)}>
                      <span className="rl-member-result__redeem-icon"><PointsStar /></span>
                      Redeem RM 10
                    </button>
                  )}
                </div>
              </article>
            );
          })}
        </div>
      </div>
    </Modal>
  );
};
