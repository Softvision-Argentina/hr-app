import { SkillTypeModule } from './skill-type.module';

describe('SkillTypeModule', () => {
  let skillTypeModule: SkillTypeModule;

  beforeEach(() => {
    skillTypeModule = new SkillTypeModule();
  });

  it('should create an instance', () => {
    expect(skillTypeModule).toBeTruthy();
  });
});
