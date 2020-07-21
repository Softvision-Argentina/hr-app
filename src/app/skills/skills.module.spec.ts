import { SkillsModule } from './skills.module';

describe('SkillsModule', () => {
  let skillsModule: SkillsModule;

  beforeEach(() => {
    skillsModule = new SkillsModule();
  });

  it('should create an instance', () => {
    expect(skillsModule).toBeTruthy();
  });
});
